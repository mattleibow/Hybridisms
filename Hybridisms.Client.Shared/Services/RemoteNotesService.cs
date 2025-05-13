using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace Hybridisms.Client.Shared.Services;

public class RemoteNotesService(HttpClient httpClient) : INotesService
{
    public async IAsyncEnumerable<Notebook> GetNotebooksAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var notebook in httpClient.GetFromJsonAsAsyncEnumerable<Notebook>("api/notebook").WithCancellation(cancellationToken))
        {
            if (notebook is null)
                continue;

            yield return notebook;
        }
    }

    public async Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<Notebook>($"api/notebook/{notebookId}", cancellationToken);
    }

    public async IAsyncEnumerable<Note> GetNotesAsync(Guid notebookId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var note in httpClient.GetFromJsonAsAsyncEnumerable<Note>($"api/notebook/{notebookId}/notes").WithCancellation(cancellationToken))
        {
            if (note is null)
                continue;

            yield return note;
        }
    }

    public async Task<Note?> GetNoteAsync(Guid noteId, CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<Note>($"api/notes/{noteId}", cancellationToken);
    }

    public async IAsyncEnumerable<Note> GetStarredNotesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var note in httpClient.GetFromJsonAsAsyncEnumerable<Note>("api/notes/starred").WithCancellation(cancellationToken))
        {
            if (note is null)
                continue;

            yield return note;
        }
    }

    public async IAsyncEnumerable<Topic> GetTopicsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var topic in httpClient.GetFromJsonAsAsyncEnumerable<Topic>("api/topics").WithCancellation(cancellationToken))
        {
            if (topic is null)
                continue;

            yield return topic;
        }
    }

    public async IAsyncEnumerable<Notebook> SaveNotebooksAsync(IEnumerable<Notebook> notebooks, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/notebook", notebooks, cancellationToken);
        response.EnsureSuccessStatusCode();

        await foreach (var notebook in response.Content.ReadFromJsonAsAsyncEnumerable<Notebook>(cancellationToken).WithCancellation(cancellationToken))
        {
            if (notebook is null)
                continue;

            yield return notebook;
        }
    }

    public async IAsyncEnumerable<Note> SaveNotesAsync(IEnumerable<Note> notes, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/notes", notes, cancellationToken);
        response.EnsureSuccessStatusCode();

        await foreach (var note in response.Content.ReadFromJsonAsAsyncEnumerable<Note>(cancellationToken).WithCancellation(cancellationToken))
        {
            if (note is null)
                continue;
                
            yield return note;
        }
    }

    public async IAsyncEnumerable<Topic> SaveTopicsAsync(IEnumerable<Topic> topics, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/topics", topics, cancellationToken);
        response.EnsureSuccessStatusCode();

        await foreach (var topic in response.Content.ReadFromJsonAsAsyncEnumerable<Topic>(cancellationToken).WithCancellation(cancellationToken))
        {
            if (topic is null)
                continue;
            yield return topic;
        }
    }
}
