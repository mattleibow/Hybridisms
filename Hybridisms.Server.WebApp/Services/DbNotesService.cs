using Hybridisms.Client.Shared.Services;
using Hybridisms.Server.WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace Hybridisms.Server.WebApp.Services;

public class DbNotesService(HybridismsDbContext db) : INotesService
{
    public async Task<IList<Note>> GetNotesAsync(int count = 5, CancellationToken cancellationToken = default)
    {
        var entities = await db.Notes
            .OrderBy(f => f.Date)
            .Take(count)
            .ToListAsync(cancellationToken);

        var notes = new List<Note>();
        foreach (var e in entities)
        {
            cancellationToken.ThrowIfCancellationRequested();

            notes.Add(new Note
            {
                Id = e.Id,
                Date = e.Date,
                TemperatureC = e.TemperatureC,
                Summary = e.Summary
            });
        }
        return notes;
    }
}
