using System.Net.Http.Json;

namespace Hybridisms.Shared.Services;

// TODO: AI - [D] Remote data service
/// <summary>
/// RemoteNotesService provides methods for accessing and manipulating notes, notebooks, and topics using the remote REST endpoints.
/// </summary>
public class RemoteNotesService(HttpClient httpClient) : INotesService
{
    // Notebook

    public async Task<ICollection<Notebook>> GetNotebooksAsync(CancellationToken cancellationToken = default)
    {
        var notebooks = await httpClient.GetFromJsonAsync<ICollection<Notebook>>("api/notebook");
        return notebooks ?? [];
    }

    public async Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<Notebook>($"api/notebook/{notebookId}", cancellationToken);
    }

    public async Task<ICollection<Notebook>> SaveNotebooksAsync(IEnumerable<Notebook> notebooks, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/notebook", notebooks, cancellationToken);
        response.EnsureSuccessStatusCode();

        var savedNotebooks = await response.Content.ReadFromJsonAsync<ICollection<Notebook>>(cancellationToken);
        return savedNotebooks ?? [];
    }

    public Task<ICollection<Note>> SaveNotebookNotesAsync(Guid notebookId, IEnumerable<Note> notes, CancellationToken cancellationToken = default) =>
        SaveNotebookNotesAsync(notebookId, notes, false, cancellationToken);

    public async Task<ICollection<Note>> SaveNotebookNotesAsync(Guid notebookId, IEnumerable<Note> notes, bool fetchAll, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync($"api/notebook/{notebookId}/notes?fetchAll={fetchAll}", notes, cancellationToken);
        response.EnsureSuccessStatusCode();

        var savedNotes = await response.Content.ReadFromJsonAsync<ICollection<Note>>(cancellationToken);
        return savedNotes ?? [];
    }


    // Note

    public async Task<ICollection<Note>> GetStarredNotesAsync(CancellationToken cancellationToken = default)
    {
        var starredNotes = await httpClient.GetFromJsonAsync<ICollection<Note>>("api/notes/starred", cancellationToken);
        return starredNotes ?? [];
    }

    public async Task<ICollection<Note>> GetNotesAsync(Guid notebookId, bool includeDeleted, CancellationToken cancellationToken = default)
    {
        var notes = await httpClient.GetFromJsonAsync<ICollection<Note>>($"api/notebook/{notebookId}/notes?includeDeleted={includeDeleted}", cancellationToken);
        return notes ?? [];
    }

    public async Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<Note>($"api/notes/{noteId}", cancellationToken);
    }

    public async Task DeleteNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.DeleteAsync($"api/notes/{noteId}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }


    // Topic

    public async Task<ICollection<Topic>> GetTopicsAsync(CancellationToken cancellationToken = default)
    {
        var topics = await httpClient.GetFromJsonAsync<ICollection<Topic>>("api/topics", cancellationToken);
        return topics ?? [];
    }

    public async Task<ICollection<Topic>> SaveTopicsAsync(IEnumerable<Topic> topics, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/topics", topics, cancellationToken);
        response.EnsureSuccessStatusCode();

        var savedTopics = await response.Content.ReadFromJsonAsync<ICollection<Topic>>(cancellationToken);
        return savedTopics ?? [];
    }
}
