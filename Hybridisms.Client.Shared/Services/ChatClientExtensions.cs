using System.Text.Json;
using Microsoft.Extensions.AI;

namespace Hybridisms.Client.Shared.Services;

public static class ChatClientExtensions
{
    public static async Task<string> GetResponseAsync(this IChatClient chatClient, string systemPrompt, string userPrompt, int? maxOutputTokens = null, CancellationToken cancellationToken = default)
    {
        var chatHistory = new List<ChatMessage>
        {
            new(ChatRole.System, systemPrompt),
            new(ChatRole.User, userPrompt)
        };

        var options = new ChatOptions
        {
            MaxOutputTokens = maxOutputTokens
        };

        var response = await chatClient.GetResponseAsync(chatHistory, options, cancellationToken);

        return response.Text ?? "";
    }

    public static async Task<T?> GetResponseAsync<T>(this IChatClient chatClient, string systemPrompt, string userPrompt, int? maxOutputTokens = null, CancellationToken cancellationToken = default)
    {
        var response = await chatClient.GetResponseAsync(systemPrompt, userPrompt, maxOutputTokens, cancellationToken);

        if (string.IsNullOrWhiteSpace(response))
            return default;

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
