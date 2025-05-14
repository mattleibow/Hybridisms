namespace Hybridisms.Client.Shared.Services;

public interface INotesService
{
    Task<ICollection<Notebook>> GetNotebooksAsync(CancellationToken cancellationToken = default);

    Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default);

    Task<ICollection<Note>> GetNotesAsync(Guid notebookId, CancellationToken cancellationToken = default);

    Task<ICollection<Note>> GetStarredNotesAsync(CancellationToken cancellationToken = default);

    Task<ICollection<Topic>> GetTopicsAsync(CancellationToken cancellationToken = default);

    Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default);

    Task<ICollection<Notebook>> SaveNotebooksAsync(IEnumerable<Notebook> notebooks, CancellationToken cancellationToken = default);

    Task<ICollection<Note>> SaveNotesAsync(IEnumerable<Note> notes, CancellationToken cancellationToken = default);

    Task<ICollection<Topic>> SaveTopicsAsync(IEnumerable<Topic> topics, CancellationToken cancellationToken = default);
}

public static class NotesServiceExtensions
{
    public static async Task<Notebook> SaveNotebookAsync(this INotesService notesService, Notebook notebook, CancellationToken cancellationToken = default)
    {
        var notebooks = await notesService.SaveNotebooksAsync([notebook], cancellationToken);
        return notebooks.FirstOrDefault() ?? throw new InvalidOperationException("No notebook was returned after saving.");
    }
    
    public static async Task<Note> SaveNoteAsync(this INotesService notesService, Note note, CancellationToken cancellationToken = default)
    {
        var notes = await notesService.SaveNotesAsync([note], cancellationToken);
        return notes.FirstOrDefault() ?? throw new InvalidOperationException("No note was returned after saving.");
    }
}
