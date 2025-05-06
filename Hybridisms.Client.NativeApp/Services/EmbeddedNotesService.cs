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

    private async Task EnsureCreatedAsync()
    {
        await db.EnsureCreatedAsync();
    }
}
