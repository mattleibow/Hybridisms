namespace Hybridisms.Client.Native.Services;

/// <summary>
/// IAppFileProvider is an interface that provides methods for accessing files within
/// the app package.
/// </summary>
public interface IAppFileProvider
{
    Task<Stream> OpenAppPackageFileAsync(string path);
}
