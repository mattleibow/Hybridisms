using System.Runtime.CompilerServices;
using Hybridisms.Client.Shared.Services;
using Hybridisms.Server.WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace Hybridisms.Server.WebApp.Services;

public class DbNotesService(HybridismsDbContext db) : INotesService
{
    public async IAsyncEnumerable<Notebook> GetNotebooksAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var entities = await db.Notebooks
            .ToListAsync(cancellationToken);

        foreach (var entity in entities)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return MapNotebook(entity);
        }
    }

    public async Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default)
    {
        var entity = await db.Notebooks
            .FirstOrDefaultAsync(n => n.Id == notebookId, cancellationToken);

        if (entity is null)
            return null;

        return (Notebook?)MapNotebook(entity);
    }

    public async IAsyncEnumerable<Note> GetNotesAsync(Guid notebookId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var notes = await db.Notes
            .Where(n => n.NotebookId == notebookId)
            .Include(n => n.Topics)
            .ToListAsync(cancellationToken);

        foreach (var note in notes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return MapNote(note);
        }
    }

    public async Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        var entity = await db.Notes
            .Include(n => n.Topics)
            .FirstOrDefaultAsync(n => n.Id == noteId, cancellationToken);

        if (entity is null)
            return null;

        return (Note?)MapNote(entity);
    }

    public async IAsyncEnumerable<Note> GetStarredNotesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var notes = await db.Notes
            .Where(n => n.Starred)
            .Include(n => n.Topics)
            .ToListAsync(cancellationToken);

        foreach (var note in notes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return MapNote(note);
        }
    }

    public async IAsyncEnumerable<Topic> GetTopicsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var topics = await db.Topics
            .ToListAsync(cancellationToken);

        foreach (var topic in topics)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return MapTopic(topic);
        }
    }

    private static Notebook MapNotebook(NotebookEntity entity) =>
        new Notebook
        {
            Id = entity.Id,
            Created = entity.Created,
            Modified = entity.Modified,
            Title = entity.Title,
            Description = entity.Description,
        };

    private static Note MapNote(NoteEntity note) =>
        new Note
        {
            Id = note.Id,
            Created = note.Created,
            Modified = note.Modified,
            Title = note.Title,
            Content = note.Content,
            Starred = note.Starred,
            Topics = note.Topics.Select(MapTopic).ToList(),
            NotebookId = note.NotebookId,
        };

    private static Topic MapTopic(TopicEntity entity) =>
        new Topic
        {
            Id = entity.Id,
            Created = entity.Created,
            Modified = entity.Modified,
            Name = entity.Name,
            Color = entity.Color,
        };
}
