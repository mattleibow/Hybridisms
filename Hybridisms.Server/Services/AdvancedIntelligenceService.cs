using Hybridisms.Shared.Services;
using Microsoft.Extensions.AI;

namespace Hybridisms.Server.Services;

// TODO: AI - [Z] Cloud OpenAI service
public class AdvancedIntelligenceService(INotesService db, IChatClient chatClient) : IIntelligenceService
{
    private sealed record SelectedLabel(string Label, string Reason);

    public async Task<ICollection<TopicRecommendation>> RecommendTopicsAsync(Note note, int count = 3, CancellationToken cancellationToken = default)
    {
        // Fetch all topics so the AI can only select from valid, existing topics.
        var allTopics = await db.GetTopicsAsync(cancellationToken);

        // If there are no topics, there's nothing to recommend—exit early to avoid unnecessary AI calls.
        if (allTopics.Count == 0)
        {
            return [];
        }

        // We pass the topic list to the AI so it doesn't invent new topics, ensuring recommendations are consistent with our data.
        var topicNames = allTopics.Select(t => t.Name).ToList();
        var topicsListString = string.Join(", ", topicNames);

        // Instruct the AI to return a JSON array of objects with label and reason for each selected topic.
        const string systemPrompt = """
            You are a helpful assistant that provides topic recommendations based on note content.
            You will be given a note and a list of available topics. Your task is to select the
            most relevant topics from the list based on the note's content.
            
            Respond in a properly formatted JSON as an array of objects, each with a "label"
            (the topic name from the list above) and a "reason" (why you selected it). Do
            not include any other text or explanations or wrapping code blocks.
            
            For example:

            [
              { "label": "Topic1", "reason": "Reason for Topic1" },
              { "label": "Topic2", "reason": "Reason for Topic2" }
            ]
            
            """;
        var prompt = $$"""
            Select the {{count}} most relevant topics for this note:

            Note content:
            {{note.Title}}

            {{note.Content}}

            Available topics:
            {{topicsListString}}
            """;

        // Use the natural language service to get the response from the AI model.
        var selectedLabels = await chatClient.GetResponseAsync<List<SelectedLabel>>(systemPrompt, prompt, null, cancellationToken);
        if (selectedLabels is null || selectedLabels.Count == 0)
        {
            return [];
        }

        // Only yield topics that actually exist in our database, ensuring data integrity.
        var recommendations = new List<TopicRecommendation>();
        foreach (var label in selectedLabels)
        {
            var topic = allTopics.FirstOrDefault(t => string.Equals(t.Name, label.Label, StringComparison.OrdinalIgnoreCase));
            if (topic is null)
                continue;

            recommendations.Add(new TopicRecommendation
            {
                Topic = topic,
                Reason = label.Reason
            });

            if (recommendations.Count >= count)
                break;
        }

        return recommendations;
    }

    public async Task<string> GenerateNoteContentsAsync(string prompt, CancellationToken cancellationToken = default)
    {
        const string systemPrompt = """
            You are a note-taking assistant that generates short notes.
            """;

        var response = await chatClient.GetResponseAsync(systemPrompt, prompt, null, cancellationToken);
        return response ?? "";
    }
}
