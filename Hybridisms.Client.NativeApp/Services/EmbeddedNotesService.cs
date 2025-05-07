using System.Runtime.CompilerServices;
using Hybridisms.Client.NativeApp.Data;
using Hybridisms.Client.Shared.Services;

namespace Hybridisms.Client.NativeApp.Services;

public class EmbeddedNotesService(HybridismsEmbeddedDbContext db) : INotesService
{
    public async IAsyncEnumerable<Notebook> GetNotebooksAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();
        var notebooks = await db.Notebooks.ToListAsync();
        foreach (var notebook in notebooks)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return await ToNotebookAsync(notebook, cancellationToken);
        }
    }

    public async Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();
        var notebook = await db.Notebooks.FirstOrDefaultAsync(n => n.Id == notebookId);
        if (notebook == null) return null;
        return await ToNotebookAsync(notebook, cancellationToken);
    }

    public async IAsyncEnumerable<Note> GetNotesAsync(Guid notebookId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();
        var notes = await db.Notes.Where(n => n.NotebookId == notebookId).ToListAsync();
        foreach (var note in notes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return await ToNoteAsync(note, cancellationToken);
        }
    }

    public async IAsyncEnumerable<Note> GetStarredNotesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();
        var notes = await db.Notes.Where(n => n.Starred).ToListAsync();
        foreach (var note in notes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return await ToNoteAsync(note, cancellationToken);
        }
    }

    public async IAsyncEnumerable<Label> GetLabelsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();
        var labels = await db.Labels.ToListAsync();
        foreach (var label in labels)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return new Label
            {
                Id = label.Id,
                Name = label.Name,
                Color = label.Color,
                Created = label.Created,
                Modified = label.Modified
            };
        }
    }

    public async Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();
        var note = await db.Notes.FirstOrDefaultAsync(n => n.Id == noteId);
        if (note == null) return null;
        return await ToNoteAsync(note, cancellationToken);
    }

    // --- Helpers ---
    private async Task<Notebook> ToNotebookAsync(NotebookEntity entity, CancellationToken cancellationToken)
    {
        var notes = await db.Notes.Where(n => n.NotebookId == entity.Id).ToListAsync();
        var noteModels = new List<Note>();
        foreach (var noteEntity in notes)
            noteModels.Add(await ToNoteAsync(noteEntity, cancellationToken));
        return new Notebook
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Created = entity.Created,
            Modified = entity.Modified,
            Notes = noteModels
        };
    }

    private async Task<Note> ToNoteAsync(NoteEntity entity, CancellationToken cancellationToken)
    {
        var noteLabelEntities = await db.NoteLabels.Where(nl => nl.NoteId == entity.Id).ToListAsync();
        var labelIds = noteLabelEntities.Select(nl => nl.LabelId).ToList();
        var labelEntities = await db.Labels.Where(l => labelIds.Contains(l.Id)).ToListAsync();
        var labels = labelEntities.Select(l => new Label
        {
            Id = l.Id,
            Name = l.Name,
            Color = l.Color,
            Created = l.Created,
            Modified = l.Modified
        }).ToList();
        return new Note
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            Starred = entity.Starred,
            NotebookId = entity.NotebookId,
            Created = entity.Created,
            Modified = entity.Modified,
            Labels = labels
        };
    }

    private async Task EnsureCreatedAsync()
    {
        await db.EnsureCreatedAsync();
    }
}
