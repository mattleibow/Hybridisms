using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Hybridisms.Client.Shared.Services;

public class RemoteNotesService(HttpClient httpClient) : INotesService
{
    public async IAsyncEnumerable<Note> GetNotesAsync(int count = 5, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var note in httpClient.GetFromJsonAsAsyncEnumerable<Note>("api/notes").WithCancellation(cancellationToken))
        {
            if (note is null)
                continue;

            yield return note;
        }
    }
}
