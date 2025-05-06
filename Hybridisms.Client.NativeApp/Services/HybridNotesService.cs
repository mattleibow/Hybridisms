using Hybridisms.Client.NativeApp.Data;
using Hybridisms.Client.Shared.Services;
using Microsoft.Extensions.Options;

namespace Hybridisms.Client.NativeApp.Services;

public class HybridNotesService(RemoteNotesService remote, EmbeddedNotesService local, IOptions<HybridismsEmbeddedDbContext.DbContextOptions> options) : INotesService
{
    public async Task<IList<Note>> GetNotesAsync(int count = 5, CancellationToken cancellationToken = default)
    {
        try
        {
            // Attempt to get notes from remote service
            var notes = await remote.GetNotesAsync(count, cancellationToken);
            if (notes?.Count > 0)
            {
                await local.SyncNotesAsync(notes);
                return notes;
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // Ignore and fall back to local
        }

        // Fallback to local

        // Make sure to seed the local database if it doesn't exist
        await CopyFromRawResourcesAsync(cancellationToken);

        // Then get notes from local service
        return await local.GetNotesAsync(count, cancellationToken);
    }

    private async Task CopyFromRawResourcesAsync(CancellationToken cancellationToken = default)
    {
        if (options.Value.DatabasePath is not string path || File.Exists(path))
            return;

        using var raw = await FileSystem.OpenAppPackageFileAsync("hybridisms.db");
        using var fileStream = File.Create(path);
        await raw.CopyToAsync(fileStream, cancellationToken);
    }
}
