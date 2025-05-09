using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace Hybridisms.Client.Shared.Services;

public class RemoteIntelligenceService(HttpClient httpClient) : IIntelligenceService
{
    public async IAsyncEnumerable<TopicRecommendation> RecommendTopicsAsync(Note note, int count = 3, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync($"api/intelligence/recommend-topics?count={count}", note, cancellationToken);
        response.EnsureSuccessStatusCode();

        await foreach (var recommendation in response.Content.ReadFromJsonAsAsyncEnumerable<TopicRecommendation>().WithCancellation(cancellationToken))
        {
            if (recommendation is null)
                continue;

            yield return recommendation;
        }
    }
}
