using System.IO;
using System.Threading.Tasks;

namespace Hybridisms.Client.Native.Services;

public interface IAppFileProvider
{
    Task<Stream> OpenAppPackageFileAsync(string path);
}
