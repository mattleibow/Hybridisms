using System.Net.Http.Json;

namespace Hybridisms.Shared.Services;

public class RemoteIntelligenceService(HttpClient httpClient) : IIntelligenceService
{
    public async Task<ICollection<TopicRecommendation>> RecommendTopicsAsync(Note note, int count = 3, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync($"api/intelligence/recommend-topics?count={count}", note, cancellationToken);
        response.EnsureSuccessStatusCode();

        var recommendations = await response.Content.ReadFromJsonAsync<ICollection<TopicRecommendation>>();
        
        return recommendations ?? [];
    }

    public async Task<string> GenerateNoteContentsAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync($"api/intelligence/generate-note-contents", prompt, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return content ?? "";
    }
}
