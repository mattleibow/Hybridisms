namespace Hybridisms.Client.Shared.Services;

public interface INotesService
{
    IAsyncEnumerable<Note> GetNotesAsync(int count = 5, CancellationToken cancellationToken = default);
}
