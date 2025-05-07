namespace Hybridisms.Client.Shared.Services;

public interface ITopicRecommendationService
{
    IAsyncEnumerable<Topic> RecommendTopicsAsync(Note note, int count = 3, CancellationToken cancellationToken = default);
}
