using Hybridisms.Client.Native.Services;

namespace Hybridisms.Client.NativeApp.Services;

public class MauiAppFileProvider : IAppFileProvider
{
    public Task<Stream> OpenAppPackageFileAsync(string path) =>
        FileSystem.OpenAppPackageFileAsync(path);
}
