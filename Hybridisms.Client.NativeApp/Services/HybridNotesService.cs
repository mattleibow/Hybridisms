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
        // await CopyFromRawResourcesAsync(cancellationToken);

        // Then get notes from local service
        await foreach (var note in local.GetNotesAsync(count).WithCancellation(cancellationToken))
        {
            await Task.Delay(100, cancellationToken); // Simulate some delay

            yield return note;
        }
    }

    public async Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default)
    {
        // Try remote first
        var remoteNotebook = await remote.GetNotebookAsync(notebookId, cancellationToken);
        if (remoteNotebook != null)
            return remoteNotebook;
        // Fallback to local
        return await local.GetNotebookAsync(notebookId, cancellationToken);
    }

    public async Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        // Try remote first
        var remoteNote = await remote.GetNoteAsync(noteId, cancellationToken);
        if (remoteNote != null)
            return remoteNote;
        // Fallback to local
        return await local.GetNoteAsync(noteId, cancellationToken);
    }

    public async IAsyncEnumerable<Notebook> GetNotebooksAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var notebook in remote.GetNotebooksAsync(cancellationToken).WithCancellation(cancellationToken))
        {
            yield return notebook;
        }
    }

    public async IAsyncEnumerable<Note> GetNotesAsync(Guid notebookId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var note in remote.GetNotesAsync(notebookId, cancellationToken).WithCancellation(cancellationToken))
        {
            yield return note;
        }
    }

    public async IAsyncEnumerable<Note> GetStarredNotesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var note in remote.GetStarredNotesAsync(cancellationToken).WithCancellation(cancellationToken))
        {
            yield return note;
        }
    }

    public async IAsyncEnumerable<Topic> GetTopicsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var topic in remote.GetTopicsAsync(cancellationToken).WithCancellation(cancellationToken))
        {
            yield return topic;
        }
    }

    private async Task<IList<Note>?> GetRemoteNotesAsync(int count, CancellationToken cancellationToken)
    {
        // This method is obsolete and not used by the interface, so it can be removed or left unused.
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
