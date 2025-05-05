using Microsoft.EntityFrameworkCore;

namespace Hybridisms.Server.WebApp.Data;

public class HybridismsDbContext(DbContextOptions<HybridismsDbContext> options) : DbContext(options)
{
    public DbSet<NoteEntity> Notes => Set<NoteEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NoteEntity>().ToTable("Notes");
    }

    public async Task SeedAsync()
    {
        if (!await Notes.AnyAsync())
        {
            var notes = GenerateSeedNotes();
            await Notes.AddRangeAsync(notes);
            await SaveChangesAsync();
        }
    }

    private static IEnumerable<NoteEntity> GenerateSeedNotes(int days = 30)
    {
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var summaries = new[]
        {
            "Freezing", "Icy", "Frosty", "Bracing",
            "Chilly", "Cool", "Mild", "Warm",
            "Balmy", "Hot", "Sweltering", "Scorching"
        };

        const int MinTemperature = -20;
        const int MaxTemperature = 55;

        for (int index = 1; index <= days; index++)
        {
            var temp = Random.Shared.Next(MinTemperature, MaxTemperature + 1);
            var summaryIndex = (temp - MinTemperature) * summaries.Length / (MaxTemperature - MinTemperature + 1);

            yield return new NoteEntity
            {
                Id = Guid.NewGuid(),
                Date = startDate.AddDays(index),
                TemperatureC = temp,
                Summary = summaries[summaryIndex]
            };
        }
    }
}