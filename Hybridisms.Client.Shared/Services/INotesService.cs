namespace Hybridisms.Client.Shared.Services;

public interface INotesService
{
    IAsyncEnumerable<Notebook> GetNotebooksAsync(CancellationToken cancellationToken = default);

    Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<Note> GetNotesAsync(Guid notebookId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<Note> GetStarredNotesAsync(CancellationToken cancellationToken = default);

    IAsyncEnumerable<Topic> GetTopicsAsync(CancellationToken cancellationToken = default);

    Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default);
}
