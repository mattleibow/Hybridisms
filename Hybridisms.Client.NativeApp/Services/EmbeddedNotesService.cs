using Hybridisms.Client.NativeApp.Data;
using Hybridisms.Client.Shared.Services;

namespace Hybridisms.Client.NativeApp.Services;

public class EmbeddedNotesService(HybridismsEmbeddedDbContext db) : INotesService
{
    public async Task<IList<Note>> GetNotesAsync(int count = 5, CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();

        var entities = await db.Notes
            .OrderBy(n => n.Date)
            .Take(count)
            .ToListAsync();

        cancellationToken.ThrowIfCancellationRequested();

        var notes = new List<Note>();
        foreach (var entity in entities)
        {
            cancellationToken.ThrowIfCancellationRequested();

            notes.Add(entity.ToNote());
        }

        return notes;
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
