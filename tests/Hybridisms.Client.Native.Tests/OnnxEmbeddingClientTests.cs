using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Hybridisms.Client.Native.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace 

public class OnnxEmbeddingClientTests
{
    [Fact]
    public async Task GetRankedMatchesAsync_ReturnsEmpty_WhenTextIsNullOrWhitespace()
    {
        var fileProvider = Substitute.For<IAppFileProvider>();
        var options = Substitute.For<IOptions<OnnxEmbeddingClient.EmbeddingClientOptions>>();
        options.Value.Returns(new OnnxEmbeddingClient.EmbeddingClientOptions { BundledPath = "model.zip", ExtractedPath = "path" });
        var logger = Substitute.For<ILogger<OnnxEmbeddingClient>>();
        var client = new OnnxEmbeddingClient(fileProvider, options, logger);

        var result = await client.GetRankedMatchesAsync<string>(" ", new List<string> { "a", "b" }, s => s);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetRankedMatchesAsync_ReturnsEmpty_WhenNoValidOptions()
    {
        var fileProvider = Substitute.For<IAppFileProvider>();
        var options = Substitute.For<IOptions<OnnxEmbeddingClient.EmbeddingClientOptions>>();
        options.Value.Returns(new OnnxEmbeddingClient.EmbeddingClientOptions { BundledPath = "model.zip", ExtractedPath = "path" });
        var logger = Substitute.For<ILogger<OnnxEmbeddingClient>>();
        var client = new OnnxEmbeddingClient(fileProvider, options, logger);

        var result = await client.GetRankedMatchesAsync<string>("test", new List<string> { "", null }, s => s);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetRankedMatchesAsync_ReturnsRankedResults_ForValidInput()
    {
        var fileProvider = Substitute.For<IAppFileProvider>();
        var options = Substitute.For<IOptions<OnnxEmbeddingClient.EmbeddingClientOptions>>();
        options.Value.Returns(new OnnxEmbeddingClient.EmbeddingClientOptions { BundledPath = "model.zip", ExtractedPath = "path" });
        var logger = Substitute.For<ILogger<OnnxEmbeddingClient>>();
        var client = new OnnxEmbeddingClient(fileProvider, options, logger);

        var optionsList = new List<string> { "apple", "banana", "carrot" };
        var result = await client.GetRankedMatchesAsync("apple", optionsList, s => s, 2);
        Assert.True(result.Count <= 2);
        Assert.All(result, r => Assert.Contains(r.Match, optionsList));
    }

    [Fact]
    public async Task GetRankedMatchesAsync_RespectsCancellation()
    {
        var fileProvider = Substitute.For<IAppFileProvider>();
        var options = Substitute.For<IOptions<OnnxEmbeddingClient.EmbeddingClientOptions>>();
        options.Value.Returns(new OnnxEmbeddingClient.EmbeddingClientOptions { BundledPath = "model.zip", ExtractedPath = "path" });
        var logger = Substitute.For<ILogger<OnnxEmbeddingClient>>();
        var client = new OnnxEmbeddingClient(fileProvider, options, logger);

        var optionsList = new List<string> { "a", "b", "c" };
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            await client.GetRankedMatchesAsync("test", optionsList, s => s, 3, cts.Token);
        });
    }

    [Fact]
    public async Task GetRankedMatchesAsync_ReturnsAll_WhenCountExceedsOptions()
    {
        var fileProvider = Substitute.For<IAppFileProvider>();
        var options = Substitute.For<IOptions<OnnxEmbeddingClient.EmbeddingClientOptions>>();
        options.Value.Returns(new OnnxEmbeddingClient.EmbeddingClientOptions { BundledPath = "model.zip", ExtractedPath = "path" });
        var logger = Substitute.For<ILogger<OnnxEmbeddingClient>>();
        var client = new OnnxEmbeddingClient(fileProvider, options, logger);

        var optionsList = new List<string> { "x", "y" };
        var result = await client.GetRankedMatchesAsync("x", optionsList, s => s, 10);
        Assert.Equal(optionsList.Count, result.Count);
    }
}
