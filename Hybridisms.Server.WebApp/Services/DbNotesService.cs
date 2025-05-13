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

        return MapNotebook(entity);
    }

    public async IAsyncEnumerable<Notebook> SaveNotebooksAsync(IEnumerable<Notebook> notebooks, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var savedNotebooks = new List<Notebook>();
        foreach (var notebook in notebooks)
        {
            var updatedNotebook = await SaveNotebookAsync(notebook, cancellationToken);
            savedNotebooks.Add(updatedNotebook);
        }
        foreach (var savedNotebook in savedNotebooks)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return savedNotebook;
        }
    }

    private async Task<Notebook> SaveNotebookAsync(Notebook notebook, CancellationToken cancellationToken)
    {
        var entity = await db.Notebooks.FirstOrDefaultAsync(n => n.Id == notebook.Id, cancellationToken);
        if (entity is null)
        {
            // If the notebook does not exist, create a new one
            // and save it to the database

            entity = MapNotebook(notebook);
            db.Notebooks.Add(entity);

            await db.SaveChangesAsync(cancellationToken);
        }
        else if (notebook.Modified > entity.Modified)
        {
            // If the incoming notebook is newer, update the entity
            // and save it to the database

            MapNotebook(notebook, entity);

            await db.SaveChangesAsync(cancellationToken);
        }
        else
        {
            // If the incoming notebook is older or the same age, do nothing
        }

        // return the updated or newly created notebook
        notebook = MapNotebook(entity);
        return notebook;
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

    public async IAsyncEnumerable<Note> SaveNotesAsync(IEnumerable<Note> notes, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var savedNotes = new List<Note>();
        foreach (var note in notes)
        {
            var updatedNote = await SaveNoteAsync(note, cancellationToken);
            savedNotes.Add(updatedNote);
        }
        foreach (var savedNote in savedNotes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return savedNote;
        }
    }

    private async Task<Note> SaveNoteAsync(Note note, CancellationToken cancellationToken)
    {
        var entity = await db.Notes.Include(n => n.Topics).FirstOrDefaultAsync(n => n.Id == note.Id, cancellationToken);
        if (entity is null)
        {
            // If the note does not exist, create a new one
            entity = MapNote(note);
            db.Notes.Add(entity);
            await db.SaveChangesAsync(cancellationToken);
        }
        else if (note.Modified > entity.Modified)
        {
            // If the incoming note is newer, update the entity
            MapNote(note, entity);
            await db.SaveChangesAsync(cancellationToken);
        }
        // else: If the incoming note is older or the same age, do nothing

        // return the updated or newly created note
        note = MapNote(entity);
        return note;
    }

    public async IAsyncEnumerable<Topic> SaveTopicsAsync(IEnumerable<Topic> topics, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var savedTopics = new List<Topic>();
        foreach (var topic in topics)
        {
            var updatedTopic = await SaveTopicAsync(topic, cancellationToken);
            savedTopics.Add(updatedTopic);
        }
        foreach (var savedTopic in savedTopics)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return savedTopic;
        }
    }

    private async Task<Topic> SaveTopicAsync(Topic topic, CancellationToken cancellationToken)
    {
        var entity = await db.Topics.FirstOrDefaultAsync(t => t.Id == topic.Id, cancellationToken);
        if (entity is null)
        {
            // If the topic does not exist, create a new one
            entity = MapTopic(topic);
            db.Topics.Add(entity);
            await db.SaveChangesAsync(cancellationToken);
        }
        else if (topic.Modified > entity.Modified)
        {
            // If the incoming topic is newer, update the entity
            MapTopic(topic, entity);
            await db.SaveChangesAsync(cancellationToken);
        }
        // else: If the incoming topic is older or the same age, do nothing

        // return the updated or newly created topic
        topic = MapTopic(entity);
        return topic;
    }

    private static Notebook MapNotebook(NotebookEntity entity) =>
        new Notebook
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Created = entity.Created,
            Modified = entity.Modified,
        };

    private static Note MapNote(NoteEntity note) =>
        new Note
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            Starred = note.Starred,
            Topics = note.Topics.Select(MapTopic).ToList(),
            NotebookId = note.NotebookId,
            Created = note.Created,
            Modified = note.Modified,
        };

    private static Topic MapTopic(TopicEntity entity) =>
        new Topic
        {
            Id = entity.Id,
            Name = entity.Name,
            Color = entity.Color,
            Created = entity.Created,
            Modified = entity.Modified,
        };

    private static NotebookEntity MapNotebook(Notebook model, NotebookEntity? entity = null)
    {
        entity ??= new NotebookEntity();
        entity.Id = model.Id;
        entity.Title = model.Title;
        entity.Description = model.Description;
        entity.Created = model.Created;
        entity.Modified = model.Modified;
        return entity;
    }

    private NoteEntity MapNote(Note model, NoteEntity? entity = null)
    {
        entity ??= new NoteEntity();
        entity.Id = model.Id;
        entity.Title = model.Title;
        entity.Content = model.Content;
        entity.Starred = model.Starred;
        entity.NotebookId = model.NotebookId;
        if (model.Topics != null)
        {
            var topics = model.Topics
                .Select(topic =>
                {
                    var topicEntity = db.Topics.FirstOrDefault(t => t.Id == topic.Id);
                    return MapTopic(topic, topicEntity);
                })
                .ToList();
            entity.Topics = topics;
        }
        entity.Created = model.Created;
        entity.Modified = model.Modified;
        return entity;
    }

    private static TopicEntity MapTopic(Topic model, TopicEntity? entity = null)
    {
        entity ??= new TopicEntity();
        entity.Id = model.Id;
        entity.Name = model.Name;
        entity.Color = model.Color;
        entity.Created = model.Created;
        entity.Modified = model.Modified;
        return entity;
    }
}
