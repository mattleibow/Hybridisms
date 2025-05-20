namespace Hybridisms.Shared.Services;

/// <summary>
/// Provides methods for managing notebooks, notes, and topics.
/// </summary>
public interface INotesService
{
    // Notebooks

    /// <summary>
    /// Gets all notebooks.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of notebooks.</returns>
    Task<ICollection<Notebook>> GetNotebooksAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a notebook by its unique identifier.
    /// </summary>
    /// <param name="notebookId">The unique identifier of the notebook.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The notebook if found; otherwise, null.</returns>
    Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves a collection of notebooks.
    /// </summary>
    /// <param name="notebooks">The notebooks to save.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The saved notebooks.</returns>
    Task<ICollection<Notebook>> SaveNotebooksAsync(IEnumerable<Notebook> notebooks, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves a collection of notes to a notebook.
    /// </summary>
    /// <param name="notebookId">The unique identifier of the notebook.</param>
    /// <param name="notes">The notes to save.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The saved notes.</returns>
    Task<ICollection<Note>> SaveNotebookNotesAsync(Guid notebookId, IEnumerable<Note> notes, CancellationToken cancellationToken = default);


    // Notes

    /// <summary>
    /// Gets all starred notes.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of starred notes.</returns>
    Task<ICollection<Note>> GetStarredNotesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all notes for a notebook.
    /// </summary>
    /// <param name="notebookId">The unique identifier of the notebook.</param>
    /// <param name="includeDeleted">Whether to include deleted notes.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of notes. If <paramref name="includeDeleted"/> is true, then the collection will include any deleted notes.</returns>
    Task<ICollection<Note>> GetNotesAsync(Guid notebookId, bool includeDeleted = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a note by its unique identifier.
    /// </summary>
    /// <param name="noteId">The unique identifier of the note.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The note if found; otherwise, null.</returns>
    Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a note by its unique identifier.
    /// </summary>
    /// <param name="noteId">The unique identifier of the note.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task DeleteNoteAsync(Guid noteId, CancellationToken cancellationToken = default);


    // Topics

    /// <summary>
    /// Gets all topics.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of topics.</returns>
    Task<ICollection<Topic>> GetTopicsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves a collection of topics.
    /// </summary>
    /// <param name="topics">The topics to save.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The saved topics.</returns>
    Task<ICollection<Topic>> SaveTopicsAsync(IEnumerable<Topic> topics, CancellationToken cancellationToken = default);
}

/// <summary>
/// Extension methods for <see cref="INotesService"/>.
/// </summary>
public static class NotesServiceExtensions
{
    /// <summary>
    /// Saves a single notebook.
    /// </summary>
    /// <param name="notesService">The notes service instance.</param>
    /// <param name="notebook">The notebook to save.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The saved notebook.</returns>
    public static async Task<Notebook> SaveNotebookAsync(this INotesService notesService, Notebook notebook, CancellationToken cancellationToken = default)
    {
        var notebooks = await notesService.SaveNotebooksAsync([notebook], cancellationToken);
        return notebooks.FirstOrDefault() ?? throw new InvalidOperationException("No notebook was returned after saving.");
    }

    /// <summary>
    /// Saves a single note to a notebook.
    /// </summary>
    /// <param name="notesService">The notes service instance.</param>
    /// <param name="notebookId">The unique identifier of the notebook.</param>
    /// <param name="note">The note to save.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The saved note.</returns>
    public static async Task<Note> SaveNotebookNoteAsync(this INotesService notesService, Guid notebookId, Note note, CancellationToken cancellationToken = default)
    {
        var notes = await notesService.SaveNotebookNotesAsync(notebookId, [note], cancellationToken);
        return notes.FirstOrDefault() ?? throw new InvalidOperationException("No note was returned after saving.");
    }
}
