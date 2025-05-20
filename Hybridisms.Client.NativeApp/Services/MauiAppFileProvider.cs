using Hybridisms.Client.Native.Services;
using Hybridisms.Shared.Services;

namespace Hybridisms.Client.NativeApp.Services;

public class MauiAppFileProvider : IAppFileProvider
{
    public Task<Stream> OpenAppPackageFileAsync(string path) =>
        FileSystem.OpenAppPackageFileAsync(path);
}
