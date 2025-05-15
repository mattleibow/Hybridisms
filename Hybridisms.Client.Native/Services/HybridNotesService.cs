using Hybridisms.Client.Native.Data;
using Hybridisms.Shared.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hybridisms.Client.Native.Services;

public class HybridNotesService(RemoteNotesService remote, EmbeddedNotesService local, IOptions<HybridismsEmbeddedDbContext.DbContextOptions> options, ILogger<HybridNotesService>? logger, IAppFileProvider fileProvider)
    : INotesService
{
    public Task<ICollection<Notebook>> GetNotebooksAsync(CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Fetching all notebooks...");

        return GetOrSyncAsync(
            local.GetNotebooksAsync,
            local.SaveNotebooksAsync,
            remote.GetNotebooksAsync,
            remote.SaveNotebooksAsync,
            cancellationToken);
    }

    public Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Fetching notebook with ID: {NotebookId}", notebookId);

        return GetOrSyncAsync(
            ct => local.GetNotebookAsync(notebookId, ct),
            local.SaveNotebookAsync,
            ct => remote.GetNotebookAsync(notebookId, ct),
            remote.SaveNotebookAsync,
            cancellationToken);
    }

    public async Task<ICollection<Notebook>> SaveNotebooksAsync(IEnumerable<Notebook> notebooks, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Saving notebooks...");

        var saved = await local.SaveNotebooksAsync(notebooks, cancellationToken);

        return saved;
    }

    public Task<ICollection<Note>> GetNotesAsync(Guid notebookId, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Fetching notes for notebook with ID: {NotebookId}", notebookId);

        return GetOrSyncAsync(
            ct => local.GetNotesAsync(notebookId, ct),
            local.SaveNotesAsync,
            ct => remote.GetNotesAsync(notebookId, ct),
            remote.SaveNotesAsync,
            cancellationToken);
    }

    public Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Fetching note with ID: {NoteId}", noteId);

        return GetOrSyncAsync(
            ct => local.GetNoteAsync(noteId, ct),
            local.SaveNoteAsync,
            ct => remote.GetNoteAsync(noteId, ct),
            remote.SaveNoteAsync,
            cancellationToken);
    }

    public async Task<ICollection<Note>> SaveNotesAsync(IEnumerable<Note> notes, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Saving notes...");

        var saved = await local.SaveNotesAsync(notes, cancellationToken);

        return saved;
    }

    public Task<ICollection<Note>> GetStarredNotesAsync(CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Fetching starred notes...");

        return GetOrSyncAsync(
            local.GetStarredNotesAsync,
            null,
            remote.GetStarredNotesAsync,
            null,
            cancellationToken);
    }

    public Task<ICollection<Topic>> GetTopicsAsync(CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Fetching all topics...");

        return GetOrSyncAsync(
            local.GetTopicsAsync,
            local.SaveTopicsAsync,
            remote.GetTopicsAsync,
            null,
            cancellationToken);
    }

    public async Task<ICollection<Topic>> SaveTopicsAsync(IEnumerable<Topic> topics, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Saving topics...");

        var saved = await local.SaveTopicsAsync(topics, cancellationToken);

        return saved;
    }

    private async Task CopyFromRawResourcesAsync(CancellationToken cancellationToken = default)
    {
        if (options.Value.DatabasePath is not string path || File.Exists(path))
            return;

        using var raw = await fileProvider.OpenAppPackageFileAsync("hybridisms.db");
        using var fileStream = File.Create(path);
        await raw.CopyToAsync(fileStream, cancellationToken);
    }

    private async Task<ICollection<T>> GetOrSyncAsync<T>(
        Func<CancellationToken, Task<ICollection<T>>> getLocal,
        Func<ICollection<T>, CancellationToken, Task<ICollection<T>>>? saveLocal,
        Func<CancellationToken, Task<ICollection<T>>> getRemote,
        Func<ICollection<T>, CancellationToken, Task<ICollection<T>>>? saveRemote,
        CancellationToken cancellationToken = default)
    {
        // Local data path check
        {
            // Load local data first, return them as they are loaded but also cache them for later
            logger?.LogInformation("Fetching local data...");
            var localData = await getLocal(cancellationToken);

            // If local data is available, save it to remote in the background
            if (localData.Count > 0)
            {
                // Start sync in background, but do not block return
                logger?.LogInformation("Syncing local data to remote...");
                _ = SyncAsync(localData, cancellationToken);
            
                return localData;
            }
        }

        // Remote data fetch
        {
            // No local data, fetch fresh from remote
            ICollection<T> remoteData;
            try
            {
                // Try to fetch remote data
                logger?.LogInformation("Fetching remote data...");
                remoteData = await getRemote(cancellationToken);
            }
            catch (Exception)
            {
                // If both fail, just bail out
                remoteData = [];
            }

            // There was remote data, so save it to local
            if (remoteData.Count > 0)
            {
                // Save remote data to local
                logger?.LogInformation("Saving remote data to local...");
                var localData = await SaveLocalAsync(remoteData, cancellationToken);

                // Return all the saved data
                return localData;
            }

            return remoteData;
        }

        async Task<ICollection<T>> SyncAsync(ICollection<T> localData, CancellationToken cancellationToken)
        {
            // Save local data to remote
            var remoteData = await SaveRemoteAsync(localData, cancellationToken);

            // Save remote data to local
            var data = await SaveLocalAsync(remoteData, cancellationToken);

            // return the synced data
            return data;
        }

        async Task<ICollection<T>> SaveRemoteAsync(ICollection<T> localData, CancellationToken cancellationToken)
        {
            if (saveRemote is null)
                return localData;

            var data = await saveRemote(localData, cancellationToken);

            return data;
        }

        async Task<ICollection<T>> SaveLocalAsync(ICollection<T> remoteData, CancellationToken cancellationToken)
        {
            if (saveLocal is null)
                return remoteData;

            var data = await saveLocal(remoteData, cancellationToken);

            return data;
        }
    }

    private async Task<T?> GetOrSyncAsync<T>(
        Func<CancellationToken, Task<T?>> getLocal,
        Func<T, CancellationToken, Task<T>>? saveLocal,
        Func<CancellationToken, Task<T?>> getRemote,
        Func<T, CancellationToken, Task<T>>? saveRemote,
        CancellationToken cancellationToken = default)
        where T : class
    {
        // Try local first
        logger?.LogInformation("Fetching local data...");
        var localItem = await getLocal(cancellationToken);
        if (localItem is not null)
        {
            // Start sync to remote in background
            logger?.LogInformation("Syncing local data to remote...");
            _ = SyncAsync(localItem, cancellationToken);
            return localItem;
        }

        // Try remote if not found locally
        T? remoteItem = null;
        try
        {
            logger?.LogInformation("Fetching remote data...");
            remoteItem = await getRemote(cancellationToken);
        }
        catch (Exception)
        {
            // Ignore and fallback
        }

        if (remoteItem is not null)
        {
            // Save remote to local
            logger?.LogInformation("Saving remote data to local...");
            var savedLocal = await SaveLocalAsync(remoteItem, cancellationToken);
            return savedLocal;
        }

        // Not found anywhere
        logger?.LogWarning("No data found locally or remotely.");
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
