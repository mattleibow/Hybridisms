using System.Text.Json;

namespace Hybridisms.Server.WebApp.Services;

public interface INaturalLanguageService
{
    Task<string?> GetResponseAsync(string prompt, CancellationToken cancellationToken = default);

    public async Task<T?> GetResponseAsync<T>(string prompt, CancellationToken cancellationToken = default)
    {
        var response = await GetResponseAsync(prompt, cancellationToken);

        if (string.IsNullOrWhiteSpace(response))
            return default;

        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            return JsonSerializer.Deserialize<T>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return default;
        }
    }
}
