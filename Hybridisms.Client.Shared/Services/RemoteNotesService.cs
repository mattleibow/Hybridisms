using System.Net.Http.Json;
using Hybridisms.Client.Shared.Services;

public class RemoteNotesService(HttpClient httpClient) : INotesService
{
    public async Task<IList<Note>> GetNotesAsync(int count = 5, CancellationToken cancellationToken = default)
    {
        var notes = new List<Note>();

        await foreach (var note in httpClient.GetFromJsonAsAsyncEnumerable<Note>("api/notes", cancellationToken))
        {
            if (note is null)
                continue;

            notes.Add(note);
        }

        return notes.ToArray();
    }
}
