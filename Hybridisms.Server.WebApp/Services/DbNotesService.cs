using System.Runtime.CompilerServices;
using Hybridisms.Client.Shared.Services;
using Hybridisms.Server.WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace Hybridisms.Server.WebApp.Services;

public class DbNotesService(HybridismsDbContext db) : INotesService
{
    public async IAsyncEnumerable<Note> GetNotesAsync(int count = 5, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var entities = await db.Notes
            .OrderBy(f => f.Date)
            .Take(count)
            .ToListAsync(cancellationToken);

        foreach (var e in entities)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Delay(100, cancellationToken); // Simulate some delay

            yield return new Note
            {
                Id = e.Id,
                Date = e.Date,
                TemperatureC = e.TemperatureC,
                Summary = e.Summary
            };
        }
    }
}
