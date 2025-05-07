using Microsoft.EntityFrameworkCore;

namespace Hybridisms.Server.WebApp.Data;

public class HybridismsDbContext(DbContextOptions<HybridismsDbContext> options) : DbContext(options)
{
    public DbSet<NoteEntity> Notes => Set<NoteEntity>();

    public DbSet<LabelEntity> Labels => Set<LabelEntity>();

    public DbSet<NotebookEntity> Notebooks => Set<NotebookEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NoteEntity>().ToTable("Notes");
        modelBuilder.Entity<LabelEntity>().ToTable("Labels");
        modelBuilder.Entity<NotebookEntity>().ToTable("Notebooks");

        modelBuilder.Entity<NoteEntity>()
            .HasMany(n => n.Labels)
            .WithMany(l => l.Notes)
            .UsingEntity(j => j.ToTable("NoteLabels"));

        modelBuilder.Entity<NotebookEntity>()
            .HasMany(nb => nb.Notes)
            .WithOne(n => n.Notebook)
            .HasForeignKey(n => n.NotebookId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    public async Task SeedAsync()
    {
        if (await Notebooks.AnyAsync())
            return;

        // Create labels with cool colors
        var foodLabel = new LabelEntity
        {
            Id = Guid.NewGuid(),
            Name = "Food",
            Color = "#FF7043" // Deep Orange
        };
        var presentationsLabel = new LabelEntity
        {
            Id = Guid.NewGuid(),
            Name = "Presentations",
            Color = "#42A5F5" // Blue
        };
        var jokesLabel = new LabelEntity
        {
            Id = Guid.NewGuid(),
            Name = "Jokes",
            Color = "#FFD600" // Yellow
        };
        var userGroupsLabel = new LabelEntity
        {
            Id = Guid.NewGuid(),
            Name = "User Groups",
            Color = "#66BB6A" // Green
        };
        var shoppingLabel = new LabelEntity
        {
            Id = Guid.NewGuid(),
            Name = "Shopping",
            Color = "#AB47BC" // Purple
        };

        // Create notebooks and notes
        var shoppingNotebook = new NotebookEntity
        {
            Id = Guid.NewGuid(),
            Title = "Shopping",
            Description = "Various shopping lists.",
            Notes =
            [
                new NoteEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Buy groceries",
                    Content =
                        """
                        - Milk
                        - Eggs
                        - Bread
                        - Coffee
                        - Pizza
                        """,
                    Labels = [foodLabel, shoppingLabel]
                },
                new NoteEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Dinner Menu",
                    Content =
                        """
                        - Roast Chicken
                        - Rolls
                        - Mayonnaise
                        - Coke
                        """,
                    Labels = [foodLabel]
                }
            ]
        };
        var cptmsdugNotebook = new NotebookEntity
        {
            Id = Guid.NewGuid(),
            Title = "CPTMSDUG",
            Description = "Notes for the Cape Town Microsoft Developer User Group.",
            Notes =
            [
                new NoteEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Prepare slides",
                    Content = "Create slides for the CPTMSDUG presentation.",
                    Labels = [presentationsLabel, userGroupsLabel]
                },
                new NoteEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Prepare demo",
                    Content = "Build and test the demo for the user group.",
                    Labels = [presentationsLabel, userGroupsLabel]
                },
                new NoteEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Opening jokes",
                    Content =
                        """
                        Possible opening jokes:

                        1. Why do programmers prefer dark mode? Because light attracts bugs!
                        2. How many programmers does it take to change a light bulb? None, that's a hardware problem.
                        3. I told my computer I needed a break, and it said 'No problem, I'll go to sleep.'
                        4. Why do Java developers wear glasses? Because they don't see sharp.
                        5. Debugging: Being the detective in a crime movie where you are also the murderer.
                        """,
                    Labels = [jokesLabel, presentationsLabel, userGroupsLabel]
                }
            ]
        };

        await Labels.AddRangeAsync(
            foodLabel,
            presentationsLabel,
            jokesLabel,
            userGroupsLabel,
            shoppingLabel);

        await Notebooks.AddRangeAsync(
            shoppingNotebook,
            cptmsdugNotebook);

        await SaveChangesAsync();
    }
}
