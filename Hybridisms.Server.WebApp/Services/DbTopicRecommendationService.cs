using System.Runtime.CompilerServices;
using Hybridisms.Client.Shared.Services;

namespace Hybridisms.Server.WebApp.Services;

public class DbTopicRecommendationService(DbNotesService db) : ITopicRecommendationService
{
    // Dummy implementation: returns 3 random topics from the database
    public async IAsyncEnumerable<Topic> RecommendTopicsAsync(Note note, int count = 3, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var allTopics = new List<Topic>();
        await foreach (var topic in db.GetTopicsAsync(cancellationToken))
        {
            allTopics.Add(topic);
        }

        if (allTopics.Count == 0)
        {
            yield break; // No topics available for recommendation
        }

        // Randomly select 3 topics
        var randomTopics = allTopics.OrderBy(_ => Guid.NewGuid()).Take(count);

        foreach (var topic in randomTopics)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return topic;
        }
    }
}
