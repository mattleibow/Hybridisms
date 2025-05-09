using System.Runtime.CompilerServices;
using Hybridisms.Client.Shared.Services;

namespace Hybridisms.Server.WebApp.Services;

public class IntelligenceService(INotesService db, INaturalLanguageService nlp) : IIntelligenceService
{
    private sealed record SelectedLabel(string Label, string Reason);

    public async IAsyncEnumerable<TopicRecommendation> RecommendTopicsAsync(Note note, int count = 3, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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
        var prompt = $$"""
            Given the following note content, select the {{count}} most relevant
            topics from the provided list.

            Note content:
            {{note.Title}}

            {{note.Content}}

            Available topics:
            {{topicsListString}}

            Respond in a properly formatted JSON as an array of objects, each with a "label"
            (the topic name from the list above) and a "reason" (why you selected it). Do
            not include any other text or explanations or wrapping code blocks.
            
            For example:

            [
              { "label": "Topic1", "reason": "Reason for Topic1" },
              { "label": "Topic2", "reason": "Reason for Topic2" }
            ]
            
            Only include up to {{count}} topics from the list above.
            """;

        // Use the natural language service to get the response from the AI model.
        var selectedLabels = await nlp.GetResponseAsync<List<SelectedLabel>>(prompt, cancellationToken);
        if (selectedLabels is null || selectedLabels.Count == 0)
        {
            yield break;
        }

        // Only yield topics that actually exist in our database, ensuring data integrity.
        foreach (var label in selectedLabels.Take(count))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var topic = allTopics.FirstOrDefault(t => string.Equals(t.Name, label.Label, StringComparison.OrdinalIgnoreCase));
            if (topic is null)
                continue;

            yield return new TopicRecommendation
            {
                Topic = topic,
                Reason = label.Reason
            };
        }
    }
}
