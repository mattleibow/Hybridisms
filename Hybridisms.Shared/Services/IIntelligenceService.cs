namespace Hybridisms.Shared.Services;

public interface IIntelligenceService
{
    Task<ICollection<TopicRecommendation>> RecommendTopicsAsync(Note note, int count = 3, CancellationToken cancellationToken = default);

    Task<string> GenerateNoteContentsAsync(string prompt, CancellationToken cancellationToken = default);
}
