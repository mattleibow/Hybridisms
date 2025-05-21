//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using Hybridisms.Client.Native.Services;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using NSubstitute;
//using Xunit;
//using Microsoft.ML.OnnxRuntimeGenAI;
//using Microsoft.Extensions.AI;

//public class OnnxChatClientTests
//{
//    [Fact]
//    public async Task GetResponseAsync_ReturnsResponse_ForValidMessages()
//    {
//        var fileProvider = Substitute.For<IAppFileProvider>();
//        var options = Substitute.For<IOptions<OnnxChatClient.ChatClientOptions>>();
//        options.Value.Returns(new OnnxChatClient.ChatClientOptions { BundledPath = "model.zip", ExtractedPath = "path" });
//        var logger = Substitute.For<ILogger<OnnxChatClient>>();
//        var client = new OnnxChatClient(fileProvider, options, logger);

//        // Minimal valid message
//        var messages = new List<ChatMessage> { new ChatMessage(ChatRole.User, "Hello!") };
//        var response = await client.GetResponseAsync(messages);
//        Assert.NotNull(response);
//        Assert.NotNull(response.Content);
//    }

//    [Fact]
//    public async Task GetResponseAsync_ThrowsOnEmptyMessages()
//    {
//        var fileProvider = Substitute.For<IAppFileProvider>();
//        var options = Substitute.For<IOptions<OnnxChatClient.ChatClientOptions>>();
//        options.Value.Returns(new OnnxChatClient.ChatClientOptions { BundledPath = "model.zip", ExtractedPath = "path" });
//        var logger = Substitute.For<ILogger<OnnxChatClient>>();
//        var client = new OnnxChatClient(fileProvider, options, logger);

//        await Assert.ThrowsAsync<System.ArgumentException>(async () =>
//        {
//            await client.GetResponseAsync(new List<ChatMessage>());
//        });
//    }

//    [Fact]
//    public async Task GetResponseAsync_CanBeCancelled()
//    {
//        var fileProvider = Substitute.For<IAppFileProvider>();
//        var options = Substitute.For<IOptions<OnnxChatClient.ChatClientOptions>>();
//        options.Value.Returns(new OnnxChatClient.ChatClientOptions { BundledPath = "model.zip", ExtractedPath = "path" });
//        var logger = Substitute.For<ILogger<OnnxChatClient>>();
//        var client = new OnnxChatClient(fileProvider, options, logger);

//        var messages = new List<ChatMessage> { new ChatMessage(ChatRole.User, "Hello!") };
//        using var cts = new CancellationTokenSource();
//        cts.Cancel();
//        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
//        {
//            await client.GetResponseAsync(messages, null, cts.Token);
//        });
//    }

//    [Fact]
//    public async Task GetStreamingResponseAsync_StreamsResponses()
//    {
//        var fileProvider = Substitute.For<IAppFileProvider>();
//        var options = Substitute.For<IOptions<OnnxChatClient.ChatClientOptions>>();
//        options.Value.Returns(new OnnxChatClient.ChatClientOptions { BundledPath = "model.zip", ExtractedPath = "path" });
//        var logger = Substitute.For<ILogger<OnnxChatClient>>();
//        var client = new OnnxChatClient(fileProvider, options, logger);

//        var messages = new List<ChatMessage> { new ChatMessage(ChatRole.User, "Stream test") };
//        var responses = new List<ChatResponseUpdate>();
//        await foreach (var update in client.GetStreamingResponseAsync(messages))
//        {
//            responses.Add(update);
//            if (responses.Count > 2) break; // Don't run forever
//        }
//        Assert.NotEmpty(responses);
//    }

//    [Fact]
//    public void Dispose_DoesNotThrow()
//    {
//        var fileProvider = Substitute.For<IAppFileProvider>();
//        var options = Substitute.For<IOptions<OnnxChatClient.ChatClientOptions>>();
//        options.Value.Returns(new OnnxChatClient.ChatClientOptions { BundledPath = "model.zip", ExtractedPath = "path" });
//        var logger = Substitute.For<ILogger<OnnxChatClient>>();
//        var client = new OnnxChatClient(fileProvider, options, logger);
//        client.Dispose();
//    }
//}
