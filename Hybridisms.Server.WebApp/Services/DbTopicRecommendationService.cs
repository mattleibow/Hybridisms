using System.Runtime.CompilerServices;
using Hybridisms.Client.Shared.Services;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.AI;

namespace Hybridisms.Server.WebApp.Services;

public class DbTopicRecommendationService(DbNotesService db, IChatClient chatClient) : ITopicRecommendationService
{
    private sealed record SelectedLabel(string Label, string Reason);

    public async IAsyncEnumerable<Topic> RecommendTopicsAsync(Note note, int count = 3, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Fetch all topics so the AI can only select from valid, existing topics.
        var allTopics = new List<Topic>();
        await foreach (var topic in db.GetTopicsAsync(cancellationToken))
        {
            allTopics.Add(topic);
        }

        // If there are no topics, there's nothing to recommend—exit early to avoid unnecessary AI calls.
        if (allTopics.Count == 0)
        {
            yield break;
        }

        // We pass the topic list to the AI so it doesn't invent new topics, ensuring recommendations are consistent with our data.
        var topicNames = allTopics.Select(t => t.Name).ToList();
        var topicsListString = string.Join(", ", topicNames);

        // Instruct the AI to return a JSON array of objects with label and reason for each selected topic.
        var prompt = $"""
            Given the following note content, select the {count} most relevant topics from the provided list.

            Note content:
            {note.Title}

            {note.Content}

            Available topics:
            {topicsListString}

            Respond in JSON format as an array of objects, each with a "label" (the topic name from the list above) and a "reason" (why you selected it). Example:
            [
              {{ "label": "Topic1", "reason": "Reason for Topic1" }},
              {{ "label": "Topic2", "reason": "Reason for Topic2" }}
            ]
            Only include up to {count} topics from the list above.
            """;

        // Using the AI to leverage its semantic understanding for better topic matching than simple keyword search.
        var suggestions = await chatClient.GetSuggestionsAsync(prompt, cancellationToken);

        // If the AI can't suggest anything, don't return irrelevant or empty results.
        if (suggestions == null || !suggestions.Any())
        {
            yield break;
        }

        // Try to parse the AI's JSON response to extract selected labels and reasons.
        List<SelectedLabel>? selectedLabels = null;
        foreach (var suggestion in suggestions)
        {
            try
            {
                selectedLabels = JsonSerializer.Deserialize<List<SelectedLabel>>(suggestion, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (selectedLabels != null && selectedLabels.Count > 0)
                    break;
            }
            catch
            {
                // Ignore parse errors and try next suggestion
            }
        }

        // If no valid JSON was returned, exit early.
        if (selectedLabels == null || selectedLabels.Count == 0)
        {
            yield break;
        }

        // Only yield topics that actually exist in our database, ensuring data integrity.
        var selectedNames = selectedLabels
            .Select(l => l.Label)
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var matchedTopics = allTopics
            .Where(topic => selectedNames.Contains(topic.Name, StringComparer.OrdinalIgnoreCase))
            .Take(count);

        foreach (var topic in matchedTopics)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return topic;
        }
    }
}
