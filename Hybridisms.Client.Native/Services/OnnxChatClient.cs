using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.ML.OnnxRuntimeGenAI;
using System.Runtime.CompilerServices;
using System.Text;

namespace Hybridisms.Client.Native.Services;

public class OnnxChatClient(IAppFileProvider fileProvider, IOptions<OnnxChatClient.ChatClientOptions> options, ILogger<OnnxChatClient>? logger)
    : OnnxModelClient<OnnxChatClient.ChatClientOptions>(fileProvider, options, logger), IChatClient
{
    private OnnxRuntimeGenAIChatClient? loadedClient;

    private async Task<OnnxRuntimeGenAIChatClient> LoadClientAsync()
    {
        if (loadedClient is not null)
            return loadedClient;

        await EnsureModelExtractedAsync();

        if (loadedClient is not null)
            return loadedClient;

        logger?.LogInformation("Loading ONNX chat model...");

        var clientOptions = new OnnxRuntimeGenAIChatClientOptions
        {
            StopSequences =
            [
                "<|system|>",
                "<|user|>",
                "<|assistant|>",
                "<|end|>"
            ],
            PromptFormatter = static (messages, options) =>
            {
                var prompt = new StringBuilder();
                foreach (var message in messages)
                {
                    foreach (var content in message.Contents.OfType<TextContent>())
                    {
                        var role = message.Role.Value.ToLowerInvariant();
                        prompt.Append("<|").Append(role).AppendLine("|>");
                        prompt.Append(content.Text).AppendLine("<|end|>");
                    }
                }
                prompt.AppendLine("<|assistant|>");
                return prompt.ToString();
            },
        };

        loadedClient = new OnnxRuntimeGenAIChatClient(options.Value.ExtractedPath, clientOptions);

        logger?.LogInformation("ONNX chat model loaded successfully.");

        return loadedClient;
    }

    public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Getting chat response...");

        var client = await LoadClientAsync();

        return await client.GetResponseAsync(messages, options, cancellationToken);
    }

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Getting streaming chat response...");
        
        var client = await LoadClientAsync();

        await foreach (var update in client.GetStreamingResponseAsync(messages, options, cancellationToken).WithCancellation(cancellationToken))
        {
            yield return update;
        }
    }

    public object? GetService(Type serviceType, object? serviceKey = null) =>
        (loadedClient as IChatClient)?.GetService(serviceType, serviceKey);

    public void Dispose() =>
        loadedClient?.Dispose();

    public class ChatClientOptions : OnnxModelClientOptions
    {
    }
}
