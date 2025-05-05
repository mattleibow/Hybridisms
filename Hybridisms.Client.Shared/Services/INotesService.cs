namespace Hybridisms.Client.Shared.Services;

public interface INotesService
{
    Task<IList<Note>> GetNotesAsync(int count = 5, CancellationToken cancellationToken = default);
}
