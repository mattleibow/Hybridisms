using Hybridisms.Client.Native.Data;
using Hybridisms.Shared.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hybridisms.Client.Native.Services;

// TODO: Data - Hybrid data service
/// <summary>
/// HybridNotesService is a hybrid data service that combines local and remote data access.
/// 
/// It first attempts to fetch data from the local SQLite database, and then syncs it with the remote service.
/// This allows the app to work offline and each time data is requested, it also checks the remote service
/// for any updates. Although the service does not ensure immediate consistency, it provides an eventual
/// consistency on the next request.
/// </summary>
public class HybridNotesService(RemoteNotesService remote, EmbeddedNotesService local, IOptions<HybridismsEmbeddedDbContext.DbContextOptions> options, ILogger<HybridNotesService>? logger, IAppFileProvider fileProvider)
    : INotesService
{
    private static readonly TimeSpan MinRetryTime = TimeSpan.FromSeconds(30);

    DateTimeOffset lastFailTime = DateTimeOffset.MinValue;

    // Notebook

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

    public async Task<ICollection<Note>> SaveNotebookNotesAsync(Guid notebookId, IEnumerable<Note> notes, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Saving notes...");

        var saved = await local.SaveNotebookNotesAsync(notebookId, notes, cancellationToken);

        return saved;
    }


    // Notes

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

    public Task<ICollection<Note>> GetNotesAsync(Guid notebookId, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Fetching notes for notebook with ID: {NotebookId}", notebookId);

        return GetOrSyncAsync(
            ct => local.GetNotesAsync(notebookId, includeDeleted: true, ct),
            (data, ct) => local.SaveNotebookNotesAsync(notebookId, data, ct),
            ct => remote.GetNotesAsync(notebookId, includeDeleted: true, ct),
            (data, ct) => remote.SaveNotebookNotesAsync(notebookId, data, fetchAll: true, ct),
            data => data.Where(n => !n.IsDeleted).ToList(),
            cancellationToken);
    }

    public Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Fetching note with ID: {NoteId}", noteId);

        return GetOrSyncAsync(
            ct => local.GetNoteAsync(noteId, ct),
            (data, ct) => local.SaveNotebookNoteAsync(data.NotebookId, data, ct),
            ct => remote.GetNoteAsync(noteId, ct),
            (data, ct) => remote.SaveNotebookNoteAsync(data.NotebookId, data, ct),
            cancellationToken);
    }

    public async Task DeleteNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Deleting note with id {ID}...", noteId);

        // Delete locally first
        await local.DeleteNoteAsync(noteId, cancellationToken);

        // If we failed too recently, don't try right away
        if (DateTimeOffset.UtcNow - lastFailTime < MinRetryTime)
        {
            logger?.LogWarning("Remote servers were unavailable too recently {TimeAgo}, skipping remote deletion.", DateTimeOffset.UtcNow - lastFailTime);
            return;
        }

        try
        {
            // Try delete remotely
            await remote.DeleteNoteAsync(noteId, cancellationToken);
        }
        catch
        {
            // ignore remote errors for offline

            lastFailTime = DateTimeOffset.UtcNow;
            logger?.LogWarning("Failed to delete note with id {ID} remotely.", noteId);
        }
    }


    // Topic

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


    // Helpers

    private async Task CopyFromRawResourcesAsync(CancellationToken cancellationToken = default)
    {
        if (options.Value.DatabasePath is not string path || File.Exists(path))
            return;

        using var raw = await fileProvider.OpenAppPackageFileAsync("hybridisms.db");
        using var fileStream = File.Create(path);
        await raw.CopyToAsync(fileStream, cancellationToken);
    }

    private Task<ICollection<T>> GetOrSyncAsync<T>(
        Func<CancellationToken, Task<ICollection<T>>> getLocal,
        Func<ICollection<T>, CancellationToken, Task<ICollection<T>>>? saveLocal,
        Func<CancellationToken, Task<ICollection<T>>> getRemote,
        Func<ICollection<T>, CancellationToken, Task<ICollection<T>>>? saveRemote,
        CancellationToken cancellationToken = default) =>
        GetOrSyncAsync<T>(getLocal, saveLocal, getRemote, saveRemote, null, cancellationToken);

    private async Task<ICollection<T>> GetOrSyncAsync<T>(
        Func<CancellationToken, Task<ICollection<T>>> getLocal,
        Func<ICollection<T>, CancellationToken, Task<ICollection<T>>>? saveLocal,
        Func<CancellationToken, Task<ICollection<T>>> getRemote,
        Func<ICollection<T>, CancellationToken, Task<ICollection<T>>>? saveRemote,
        Func<ICollection<T>, ICollection<T>>? filterData,
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

                // Return the data
                return Filter(localData);
            }
        }

        // If we failed too recently, don't try right away
        if (DateTimeOffset.UtcNow - lastFailTime < MinRetryTime)
        {
            logger?.LogWarning("Remote servers were unavailable too recently {TimeAgo}, skipping remote fetch.", DateTimeOffset.UtcNow - lastFailTime);
            return [];
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
            catch
            {
                // If both fail, just bail out
                remoteData = [];

                lastFailTime = DateTimeOffset.UtcNow;
                logger?.LogWarning("Failed to fetch remote data.");
            }

            // There was remote data, so save it to local
            if (remoteData.Count > 0)
            {
                // Save remote data to local
                logger?.LogInformation("Saving remote data to local...");
                var localData = await SaveLocalAsync(remoteData, cancellationToken);

                // Return all the saved data
                return Filter(localData);
            }

            return Filter(remoteData);
        }

        ICollection<T> Filter(ICollection<T> data)
        {
            // If there is no filter, return the data as is
            if (filterData is null)
                return data;

            // Filter the data using the provided filter function
            var filtered = filterData(data);

            return filtered;
        }

        async Task SyncAsync(ICollection<T> localData, CancellationToken cancellationToken)
        {
            // If we failed too recently, don't try right away
            if (DateTimeOffset.UtcNow - lastFailTime < MinRetryTime)
            {
                logger?.LogWarning("Remote servers were unavailable too recently {TimeAgo}, skipping data sync.", DateTimeOffset.UtcNow - lastFailTime);
                return;
            }

            try
            {
                // Save local data to remote
                var remoteData = await SaveRemoteAsync(localData, cancellationToken);

                // Save remote data to local
                var data = await SaveLocalAsync(remoteData, cancellationToken);
            }
            catch
            {
                lastFailTime = DateTimeOffset.UtcNow;
                logger?.LogWarning("Failed to sync data.");
            }
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

        // If we failed too recently, don't try right away
        if (DateTimeOffset.UtcNow - lastFailTime < MinRetryTime)
        {
            logger?.LogWarning("Remote servers were unavailable too recently {TimeAgo}, skipping remote fetch.", DateTimeOffset.UtcNow - lastFailTime);
            return null;
        }

        // Try remote if not found locally
        T? remoteItem = null;
        try
        {
            logger?.LogInformation("Fetching remote data...");
            remoteItem = await getRemote(cancellationToken);
        }
        catch
        {
            // Ignore and fallback

            lastFailTime = DateTimeOffset.UtcNow;
            logger?.LogWarning("Failed to fetch remote data.");
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

        async Task SyncAsync(T localData, CancellationToken cancellationToken)
        {
            try
            {
                // Save local data to remote
                var remoteData = await SaveRemoteAsync(localData, cancellationToken);

                // Save remote data to local
                var data = await SaveLocalAsync(remoteData, cancellationToken);
            }
            catch
            {
                lastFailTime = DateTimeOffset.UtcNow;
                logger?.LogWarning("Failed to sync data.");
            }
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
