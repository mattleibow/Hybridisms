using System.Runtime.CompilerServices;
using Hybridisms.Client.NativeApp.Data;
using Hybridisms.Client.Shared.Services;

namespace Hybridisms.Client.NativeApp.Services;

public class EmbeddedNotesService(HybridismsEmbeddedDbContext db) : INotesService
{
    private Task? ensureCreatedTask;
    private Task EnsureCreatedAsync()
    {
        return ensureCreatedTask ??= EnsureCreatedActualAsync();

        async Task EnsureCreatedActualAsync()
        {
            await db.EnsureCreatedAsync();
        }
    }

    public async IAsyncEnumerable<Notebook> GetNotebooksAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();

        var entities = await db.Notebooks
            .ToListAsync();

        foreach (var entity in entities)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return MapNotebook(entity);
        }
    }

    public async Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();

        var entity = await db.Notebooks
            .FirstOrDefaultAsync(n => n.Id == notebookId);

        if (entity is null)
            return null;

        return MapNotebook(entity);
    }

    public async IAsyncEnumerable<Notebook> SaveNotebooksAsync(IEnumerable<Notebook> notebooks, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();

        var savedEntities = new List<NotebookEntity>();
        foreach (var notebook in notebooks)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var entity = await db.Notebooks.FirstOrDefaultAsync(n => n.Id == notebook.Id);
            entity = MapNotebook(notebook, entity);
            savedEntities.Add(entity);
        }

        await db.Notebooks.InsertOrReplaceAllAsync(savedEntities);
        
        foreach (var notebook in notebooks)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return notebook;
        }
    }

    public async IAsyncEnumerable<Note> GetNotesAsync(Guid notebookId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();
        
        var notes = await db.Notes
            .Where(n => n.NotebookId == notebookId)
            .ToListAsync();

        foreach (var note in notes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return await MapNoteTopicsAsync(note, cancellationToken);
        }
    }

    public async Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();

        var note = await db.Notes
            .FirstOrDefaultAsync(n => n.Id == noteId);

        if (note is null)
            return null;

        return await MapNoteTopicsAsync(note, cancellationToken);
    }

    public async IAsyncEnumerable<Note> GetStarredNotesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();

        var notes = await db.Notes
            .Where(n => n.Starred)
            .ToListAsync();

        foreach (var note in notes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return await MapNoteTopicsAsync(note, cancellationToken);
        }
    }

    public async IAsyncEnumerable<Topic> GetTopicsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();

        var topics = await db.Topics
            .ToListAsync();

        foreach (var topic in topics)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return MapTopic(topic);
        }
    }

    public async IAsyncEnumerable<Note> SaveNotesAsync(IEnumerable<Note> notes, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();

        var savedEntities = new List<NoteEntity>();
        foreach (var note in notes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await foreach (var topic in SaveTopicsAsync(note.Topics, cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                // No need to do anything with the topic, just ensure it is saved
            }

            var entity = await db.Notes.FirstOrDefaultAsync(n => n.Id == note.Id);
            entity = await MapNoteTopicsAsync(note, entity, cancellationToken);
            savedEntities.Add(entity);
        }

        await db.Notes.InsertOrReplaceAllAsync(savedEntities);

        foreach (var note in notes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return note;
        }
    }

    public async IAsyncEnumerable<Topic> SaveTopicsAsync(IEnumerable<Topic> topics, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await EnsureCreatedAsync();

        var savedEntities = new List<TopicEntity>();
        foreach (var topic in topics)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var entity = await db.Topics.FirstOrDefaultAsync(t => t.Id == topic.Id);
            entity = MapTopic(topic, entity);
            savedEntities.Add(entity);
        }

        await db.Topics.InsertOrReplaceAllAsync(savedEntities);

        foreach (var topic in topics)
        {
            cancellationToken.ThrowIfCancellationRequested();

            yield return topic;
        }
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
            Topics = note.Topics is null ? [] : note.Topics.Select(MapTopic).ToList(),
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

    private async Task<Note> MapNoteTopicsAsync(NoteEntity note, CancellationToken cancellationToken = default)
    {
        var noteTopics = await db.NoteTopics
            .Where(nt => nt.NoteId == note.Id)
            .ToListAsync();
        var noteTopicIds = noteTopics
            .Select(nt => nt.TopicId)
            .ToList();

        cancellationToken.ThrowIfCancellationRequested();

        var topicEntities = await db.Topics
            .Where(t => noteTopicIds.Contains(t.Id))
            .ToListAsync();

        cancellationToken.ThrowIfCancellationRequested();

        note.Topics = topicEntities;

        return MapNote(note);
    }

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

    private async Task<NoteEntity> MapNoteTopicsAsync(Note model, NoteEntity? entity = null, CancellationToken cancellationToken = default)
    {
        var noteTopicIds = model.Topics
            .Select(t => t.Id)
            .ToList();

        cancellationToken.ThrowIfCancellationRequested();

        var topicEntities = await db.Topics
            .Where(t => noteTopicIds.Contains(t.Id))
            .ToListAsync();

        cancellationToken.ThrowIfCancellationRequested();

        entity ??= new NoteEntity();
        entity.Topics = topicEntities;

        return MapNote(model, entity);
    }

    private static NoteEntity MapNote(Note model, NoteEntity? entity)
    {
        entity ??= new NoteEntity();
        entity.Id = model.Id;
        entity.Title = model.Title;
        entity.Content = model.Content;
        entity.Starred = model.Starred;
        entity.NotebookId = model.NotebookId;
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
