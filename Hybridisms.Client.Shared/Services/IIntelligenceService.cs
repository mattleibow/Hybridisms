namespace Hybridisms.Client.Shared.Services;

public interface IIntelligenceService
{
    IAsyncEnumerable<TopicRecommendation> RecommendTopicsAsync(Note note, int count = 3, CancellationToken cancellationToken = default);

    IAsyncEnumerable<string> StreamNoteContentsAsync(string prompt, CancellationToken cancellationToken = default);
}
