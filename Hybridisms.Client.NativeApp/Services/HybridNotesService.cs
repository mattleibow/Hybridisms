using System.Runtime.CompilerServices;
using Hybridisms.Client.NativeApp.Data;
using Hybridisms.Client.Shared.Services;
using Microsoft.Extensions.Options;

namespace Hybridisms.Client.NativeApp.Services;

public class HybridNotesService(RemoteNotesService remote, EmbeddedNotesService local, IOptions<HybridismsEmbeddedDbContext.DbContextOptions> options) : INotesService
{
    public async IAsyncEnumerable<Note> GetNotesAsync(int count = 5, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Attempt to get notes from remote service
        var notes = await GetRemoteNotesAsync(count, cancellationToken);
        if (notes is not null)
        {
            // Sync notes to local service
            await local.SyncNotesAsync(notes);

            // Then return notes from remote service
            foreach (var note in notes)
            {
                await Task.Delay(100, cancellationToken); // Simulate some delay

                yield return note;
            }

            yield break;
        }

        // If some error occurs, fall back to local service

        // Make sure to seed the local database if it doesn't exist
        await CopyFromRawResourcesAsync(cancellationToken);

        // Then get notes from local service
        await foreach (var note in local.GetNotesAsync(count).WithCancellation(cancellationToken))
        {
            await Task.Delay(100, cancellationToken); // Simulate some delay

            yield return note;
        }
    }

    private async Task<IList<Note>?> GetRemoteNotesAsync(int count, CancellationToken cancellationToken)
    {
        try
        {
            var notes = new List<Note>();
            await foreach (var note in remote.GetNotesAsync(count).WithCancellation(cancellationToken))
            {
                notes.Add(note);
            }
            return notes;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // Ignore and fall back to local
        }

        return null;
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
