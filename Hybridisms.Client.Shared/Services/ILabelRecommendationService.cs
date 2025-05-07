namespace Hybridisms.Client.Shared.Services;

public interface ILabelRecommendationService
{
    IAsyncEnumerable<Label> RecommendLabelsAsync(Note note, int count = 3, CancellationToken cancellationToken = default);
}
