using System.Runtime.CompilerServices;
using Hybridisms.Client.NativeApp.Data;
using Hybridisms.Client.Shared.Services;
using Microsoft.Extensions.Options;

namespace Hybridisms.Client.NativeApp.Services;

public class HybridNotesService(RemoteNotesService remote, EmbeddedNotesService local, IOptions<HybridismsEmbeddedDbContext.DbContextOptions> options) : INotesService
{
    public IAsyncEnumerable<Notebook> GetNotebooksAsync(CancellationToken cancellationToken = default) =>
        GetOrSyncAsync(
            local.GetNotebooksAsync,
            local.SaveNotebooksAsync,
            remote.GetNotebooksAsync,
            remote.SaveNotebooksAsync,
            cancellationToken);

    public Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default) =>
        GetOrSyncAsync(
            ct => local.GetNotebookAsync(notebookId, ct),
            local.SaveNotebookAsync,
            ct => remote.GetNotebookAsync(notebookId, ct),
            remote.SaveNotebookAsync,
            cancellationToken);

    public async IAsyncEnumerable<Notebook> SaveNotebooksAsync(IEnumerable<Notebook> notebooks, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var notebook in local.SaveNotebooksAsync(notebooks, cancellationToken).WithCancellation(cancellationToken))
        {
            yield return notebook;
        }
    }

    public IAsyncEnumerable<Note> GetNotesAsync(Guid notebookId, CancellationToken cancellationToken = default) =>
        GetOrSyncAsync(
            ct => local.GetNotesAsync(notebookId, ct),
            local.SaveNotesAsync,
            ct => remote.GetNotesAsync(notebookId, ct),
            remote.SaveNotesAsync,
            cancellationToken);

    public Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default) =>
        GetOrSyncAsync(
            ct => local.GetNoteAsync(noteId, ct),
            local.SaveNoteAsync,
            ct => remote.GetNoteAsync(noteId, ct),
            remote.SaveNoteAsync,
            cancellationToken);

    public async IAsyncEnumerable<Note> SaveNotesAsync(IEnumerable<Note> notes, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var note in local.SaveNotesAsync(notes, cancellationToken).WithCancellation(cancellationToken))
        {
            yield return note;
        }
    }

    public IAsyncEnumerable<Note> GetStarredNotesAsync(CancellationToken cancellationToken = default) =>
        GetOrSyncAsync(
            local.GetStarredNotesAsync,
            null,
            remote.GetStarredNotesAsync,
            null,
            cancellationToken);

    public async IAsyncEnumerable<Topic> GetTopicsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var topic in remote.GetTopicsAsync(cancellationToken).WithCancellation(cancellationToken))
        {
            yield return topic;
        }
    }

    private async Task CopyFromRawResourcesAsync(CancellationToken cancellationToken = default)
    {
        if (options.Value.DatabasePath is not string path || File.Exists(path))
            return;

        using var raw = await FileSystem.OpenAppPackageFileAsync("hybridisms.db");
        using var fileStream = File.Create(path);
        await raw.CopyToAsync(fileStream, cancellationToken);
    }

    private static async IAsyncEnumerable<T> GetOrSyncAsync<T>(
        Func<CancellationToken, IAsyncEnumerable<T>> getLocal,
        Func<IEnumerable<T>, CancellationToken, IAsyncEnumerable<T>>? saveLocal,
        Func<CancellationToken, IAsyncEnumerable<T>> getRemote,
        Func<IEnumerable<T>, CancellationToken, IAsyncEnumerable<T>>? saveRemote,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Local data path check
        {
            // Load local data first, return them as they are loaded but also cache them for later
            var localData = new List<T>();
            await foreach (var data in getLocal(cancellationToken).WithCancellation(cancellationToken))
            {
                localData.Add(data);
                yield return data;
            }

            // If local data is available, save it to remote in the background
            if (localData.Count > 0)
            {
                // Start sync in background, but do not block return
                _ = SyncAsync(localData, cancellationToken);
                yield break;
            }
        }

        // Remote data fetch
        {
            // No local data, fetch fresh from remote
            var remoteData = new List<T>();
            try
            {
                // Try to fetch remote data
                await foreach (var data in getRemote(cancellationToken).WithCancellation(cancellationToken))
                {
                    remoteData.Add(data);
                }
            }
            catch (Exception)
            {
                // If both fail, just bail out
            }

            // There was remote data, so save it to local
            if (remoteData.Count > 0)
            {
                // Save remote data to local
                var localData = await SaveLocalAsync(remoteData, cancellationToken);

                // Return all the saved data
                foreach (var data in localData)
                {
                    yield return data;
                }
            }
        }

        async Task<IList<T>> SyncAsync(IEnumerable<T> localData, CancellationToken cancellationToken)
        {
            // Save local data to remote
            var remoteData = await SaveRemoteAsync(localData, cancellationToken);

            // Save remote data to local
            var data = await SaveLocalAsync(remoteData, cancellationToken);

            // return the synced data
            return data;
        }

        async Task<IList<T>> SaveRemoteAsync(IEnumerable<T> localData, CancellationToken cancellationToken)
        {
            if (saveRemote is null)
                return localData.ToList();

            var data = new List<T>();
            await foreach (var remoteData in saveRemote(localData, cancellationToken).WithCancellation(cancellationToken))
            {
                data.Add(remoteData);
            }

            return data;
        }

        async Task<IList<T>> SaveLocalAsync(IEnumerable<T> remoteData, CancellationToken cancellationToken)
        {
            if (saveLocal is null)
                return remoteData.ToList();

            var data = new List<T>();
            await foreach (var localData in saveLocal(remoteData, cancellationToken).WithCancellation(cancellationToken))
            {
                data.Add(localData);
            }

            return data;
        }
    }

    private static async Task<T?> GetOrSyncAsync<T>(
        Func<CancellationToken, Task<T?>> getLocal,
        Func<T, CancellationToken, Task<T>>? saveLocal,
        Func<CancellationToken, Task<T?>> getRemote,
        Func<T, CancellationToken, Task<T>>? saveRemote,
        CancellationToken cancellationToken = default)
        where T : class
    {
        // Try local first
        var localItem = await getLocal(cancellationToken);
        if (localItem is not null)
        {
            // Start sync to remote in background
            _ = SyncAsync(localItem, cancellationToken);
            return localItem;
        }

        // Try remote if not found locally
        T? remoteItem = null;
        try
        {
            remoteItem = await getRemote(cancellationToken);
        }
        catch (Exception)
        {
            // Ignore and fallback
        }

        if (remoteItem is not null)
        {
            // Save remote to local
            var savedLocal = await SaveLocalAsync(remoteItem, cancellationToken);
            return savedLocal;
        }

        // Not found anywhere
        return null;

        async Task<T> SyncAsync(T localData, CancellationToken cancellationToken)
        {
            // Save local data to remote
            var remoteData = await SaveRemoteAsync(localData, cancellationToken);

            // Save remote data to local
            var data = await SaveLocalAsync(remoteData, cancellationToken);

            // return the synced data
            return data;
        }

        async Task<T> SaveRemoteAsync(T localData, CancellationToken cancellationToken)
        {
            if (saveRemote is null)
                return localData;

            var data = await saveRemote(localData, cancellationToken);

            return data;
        }

        async Task<T> SaveLocalAsync(T remoteData, CancellationToken cancellationToken)
        {
            if (saveLocal is null)
                return remoteData;

            var data = await saveLocal(remoteData, cancellationToken);

            return data;
        }
    }
}
