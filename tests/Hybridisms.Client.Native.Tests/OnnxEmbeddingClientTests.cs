using Hybridisms.Client.Native.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace Hybridisms.Client.Native.Tests;

public partial class OnnxEmbeddingClientTests
{
    public class OnnxEmbeddingClientTestBase(ITestOutputHelper output) : OnnxClientTestBase<OnnxEmbeddingClient>(output)
    {
        protected OnnxEmbeddingClient CreateClient(string model, [CallerMemberName] string testMethodName = null!) =>
            CreateClient(
                model,
                (bundled, extracted) => new OnnxEmbeddingClient.EmbeddingClientOptions
                {
                    BundledPath = bundled,
                    ExtractedPath = extracted
                },
                (fileProvider, opts) => new OnnxEmbeddingClient(fileProvider, opts, null),
                testMethodName);
    }
}
