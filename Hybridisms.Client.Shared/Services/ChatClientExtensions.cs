using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.AI;

namespace Hybridisms.Server.WebApp.Services;

public static class ChatClientExtensions
{
    public static async IAsyncEnumerable<string> GetStreamingResponseAsync(this IChatClient chatClient, string systemPrompt, string userPrompt, int? maxOutputTokens = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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
        await foreach (var update in chatClient.GetStreamingResponseAsync(chatHistory, options, cancellationToken))
        {
            yield return update.Text;
        }
    }

    public static async Task<string> GetResponseAsync(this IChatClient chatClient, string systemPrompt, string userPrompt, int? maxOutputTokens = null, CancellationToken cancellationToken = default)
    {
        var sb = new StringBuilder();
        await foreach (var update in chatClient.GetStreamingResponseAsync(systemPrompt, userPrompt, maxOutputTokens, cancellationToken).WithCancellation(cancellationToken))
        {
            sb.Append(update ?? "");
        }
        return sb.ToString();
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
