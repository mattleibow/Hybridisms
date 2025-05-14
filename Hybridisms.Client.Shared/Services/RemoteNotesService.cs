using System.Net.Http.Json;

namespace Hybridisms.Client.Shared.Services;

public class RemoteNotesService(HttpClient httpClient) : INotesService
{
    public async Task<ICollection<Notebook>> GetNotebooksAsync(CancellationToken cancellationToken = default)
    {
        var notebooks = await httpClient.GetFromJsonAsync<ICollection<Notebook>>("api/notebook");
        return notebooks ?? [];
    }

    public async Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<Notebook>($"api/notebook/{notebookId}", cancellationToken);
    }

    public async Task<ICollection<Note>> GetNotesAsync(Guid notebookId, CancellationToken cancellationToken = default)
    {
        var notes = await httpClient.GetFromJsonAsync<ICollection<Note>>($"api/notebook/{notebookId}/notes", cancellationToken);
        return notes ?? [];
    }

    public async Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<Note>($"api/notes/{noteId}", cancellationToken);
    }

    public async Task<ICollection<Note>> GetStarredNotesAsync(CancellationToken cancellationToken = default)
    {
        var starredNotes = await httpClient.GetFromJsonAsync<ICollection<Note>>("api/notes/starred", cancellationToken);
        return starredNotes ?? [];
    }

    public async Task<ICollection<Topic>> GetTopicsAsync(CancellationToken cancellationToken = default)
    {
        var topics = await httpClient.GetFromJsonAsync<ICollection<Topic>>("api/topics", cancellationToken);
        return topics ?? [];
    }

    public async Task<ICollection<Notebook>> SaveNotebooksAsync(IEnumerable<Notebook> notebooks, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/notebook", notebooks, cancellationToken);
        response.EnsureSuccessStatusCode();

        var savedNotebooks = await response.Content.ReadFromJsonAsync<ICollection<Notebook>>(cancellationToken);
        return savedNotebooks ?? [];
    }

    public async Task<ICollection<Note>> SaveNotesAsync(IEnumerable<Note> notes, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/notes", notes, cancellationToken);
        response.EnsureSuccessStatusCode();

        var savedNotes = await response.Content.ReadFromJsonAsync<ICollection<Note>>(cancellationToken);
        return savedNotes ?? [];
    }

    public async Task<ICollection<Topic>> SaveTopicsAsync(IEnumerable<Topic> topics, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/topics", topics, cancellationToken);
        response.EnsureSuccessStatusCode();

        var savedTopics = await response.Content.ReadFromJsonAsync<ICollection<Topic>>(cancellationToken);
        return savedTopics ?? [];
    }
}
