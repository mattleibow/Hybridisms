using Microsoft.Extensions.AI;

namespace Hybridisms.Server.WebApp.Services;

public class OpenAINaturalLanguageService(IChatClient chatClient) : INaturalLanguageService
{
    public async Task<string?> GetResponseAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var response = await chatClient.GetResponseAsync(prompt, new ChatOptions(), cancellationToken);
        return response?.Text;
    }
}
