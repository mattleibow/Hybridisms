using Hybridisms.Client.Native.Services;
using Hybridisms.Client.Native.Tests;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

public partial class OnnxChatClientTests
{
    public class OnnxChatClientTestBase(ITestOutputHelper output) : OnnxClientTestBase<OnnxChatClient>(output)
    {
        protected OnnxChatClient CreateClient(string model, [CallerMemberName] string testMethodName = null!) =>
            CreateClient(
                model,
                (bundled, extracted) => new OnnxChatClient.ChatClientOptions
                {
                    BundledPath = bundled,
                    ExtractedPath = extracted
                },
                (fileProvider, opts) => new OnnxChatClient(fileProvider, opts, null),
                testMethodName);
    }
}
