using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace Hybridisms.Client.Shared.Services;

public class RemoteLabelRecommendationService(HttpClient httpClient) : ILabelRecommendationService
{
    public async IAsyncEnumerable<Label> RecommendLabelsAsync(Note note, int count = 3, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync($"api/recommendation/labels?count={count}", note, cancellationToken);
        response.EnsureSuccessStatusCode();

        await foreach (var label in response.Content.ReadFromJsonAsAsyncEnumerable<Label>().WithCancellation(cancellationToken))
        {
            if (label is null)
                continue;

            yield return label;
        }
    }
}
