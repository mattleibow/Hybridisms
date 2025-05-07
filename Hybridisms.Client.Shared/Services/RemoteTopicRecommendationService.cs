using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace Hybridisms.Client.Shared.Services;

public class RemoteTopicRecommendationService(HttpClient httpClient) : ITopicRecommendationService
{
    public async IAsyncEnumerable<Topic> RecommendTopicsAsync(Note note, int count = 3, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync($"api/recommendation/topics?count={count}", note, cancellationToken);
        response.EnsureSuccessStatusCode();

        await foreach (var topic in response.Content.ReadFromJsonAsAsyncEnumerable<Topic>().WithCancellation(cancellationToken))
        {
            if (topic is null)
                continue;

            yield return topic;
        }
    }
}
