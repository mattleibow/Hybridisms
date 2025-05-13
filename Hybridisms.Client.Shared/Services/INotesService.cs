namespace Hybridisms.Client.Shared.Services;

public interface INotesService
{
    IAsyncEnumerable<Notebook> GetNotebooksAsync(CancellationToken cancellationToken = default);

    Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<Note> GetNotesAsync(Guid notebookId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<Note> GetStarredNotesAsync(CancellationToken cancellationToken = default);

    IAsyncEnumerable<Topic> GetTopicsAsync(CancellationToken cancellationToken = default);

    Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<Notebook> SaveNotebooksAsync(IEnumerable<Notebook> notebooks, CancellationToken cancellationToken = default);

    IAsyncEnumerable<Note> SaveNotesAsync(IEnumerable<Note> notes, CancellationToken cancellationToken = default);

    IAsyncEnumerable<Topic> SaveTopicsAsync(IEnumerable<Topic> topics, CancellationToken cancellationToken = default);
}

public static class NotesServiceExtensions
{
    public static async Task<Notebook> SaveNotebookAsync(this INotesService notesService, Notebook notebook, CancellationToken cancellationToken = default)
    {
        await foreach (var savedNotebook in notesService.SaveNotebooksAsync([notebook], cancellationToken).WithCancellation(cancellationToken))
        {
            return savedNotebook;
        }
        throw new InvalidOperationException("No notebook was returned after saving.");
    }
    
    public static async Task<Note> SaveNoteAsync(this INotesService notesService, Note note, CancellationToken cancellationToken = default)
    {
        await foreach (var savedNote in notesService.SaveNotesAsync([note], cancellationToken).WithCancellation(cancellationToken))
        {
            return savedNote;
        }
        throw new InvalidOperationException("No note was returned after saving.");
    }
}
