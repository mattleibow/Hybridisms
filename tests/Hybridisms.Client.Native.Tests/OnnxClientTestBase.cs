using Hybridisms.Client.Native.Services;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace Hybridisms.Client.Native.Tests;

public abstract class OnnxClientTestBase<TClient>(ITestOutputHelper output) : IDisposable
{
    string? extractedPath;

    protected TClient CreateClient<TOptions>(string model, Func<string, string, TOptions> optionsCtor, Func<IAppFileProvider, IOptions<TOptions>, TClient> ctor, [CallerMemberName] string testMethodName = null!)
        where TOptions : OnnxModelClient.OnnxModelClientOptions
    {
        extractedPath = Path.Combine(
            Path.GetTempPath(),
            GetType().Namespace ?? "Hybridisms.Client.Native.Tests",
            GetType().Name,
            testMethodName,
            Guid.NewGuid().ToString());

        output.WriteLine($"Will extract model {model} to path: {extractedPath}");

        var fileProvider = Substitute.For<IAppFileProvider>();
        fileProvider.OpenAppPackageFileAsync(Arg.Any<string>())
            .Returns(call => File.OpenRead(call.Arg<string>()));

        var options = Substitute.For<IOptions<TOptions>>();
        options.Value.Returns(optionsCtor(
            $"Resources/Models/{model}_model.zip",
            extractedPath));

        var client = ctor(fileProvider, options);

        return client;
    }

    public void Dispose()
    {
        if (extractedPath is not null && Directory.Exists(extractedPath))
        {
            try
            {
                Directory.Delete(extractedPath, true);
            }
            catch (Exception ex)
            {
                output.WriteLine($"Failed to delete directory {extractedPath}: {ex.Message}");
            }
        }
    }
}