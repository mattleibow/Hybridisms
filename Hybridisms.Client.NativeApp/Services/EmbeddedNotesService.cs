using System.Runtime.CompilerServices;
using Hybridisms.Client.NativeApp.Data;
using Hybridisms.Client.Shared.Services;

namespace Hybridisms.Client.NativeApp.Services;

public class EmbeddedNotesService(HybridismsEmbeddedDbContext db) : INotesService
{
    public async IAsyncEnumerable<Note> GetNotesAsync(int count = 5, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();

        var entities = await db.Notes
            .OrderBy(n => n.Date)
            .Take(count)
            .ToListAsync();

        cancellationToken.ThrowIfCancellationRequested();

        foreach (var entity in entities)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return entity.ToNote();
        }
    }

    public async Task SyncNotesAsync(IEnumerable<Note> notes)
    {
        await EnsureCreatedAsync();

        var entities = notes.Select(NoteEntity.FromNote);

        await db.Notes.DeleteAllAsync();
        await db.Notes.InsertAllAsync(entities);
    }

    public async Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();
        // Assuming a Notebooks table exists, otherwise return null
        // If not implemented, return null for now
        return null;
    }

    public async IAsyncEnumerable<Notebook> GetNotebooksAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Notebooks are not implemented in embedded DB, so return empty
        await EnsureCreatedAsync();
        yield break;
    }

    public async IAsyncEnumerable<Note> GetNotesAsync(Guid notebookId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();
        // No notebook support, so return all notes
        var entities = await db.Notes.ToListAsync();
        cancellationToken.ThrowIfCancellationRequested();
        foreach (var entity in entities)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return entity.ToNote();
        }
    }

    public async IAsyncEnumerable<Note> GetStarredNotesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();
        // No starred support, so return empty
        yield break;
    }

    public async IAsyncEnumerable<Hybridisms.Client.Shared.Services.Label> GetLabelsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // No label support, so return empty
        yield break;
    }

    public async Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();
        var entity = await db.Connection.Table<NoteEntity>().FirstOrDefaultAsync(n => n.Id == noteId);
        return entity?.ToNote();
    }

    private async Task EnsureCreatedAsync()
    {
        await db.EnsureCreatedAsync();
    }
}
