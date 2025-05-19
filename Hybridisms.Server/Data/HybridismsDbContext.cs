using Microsoft.EntityFrameworkCore;

namespace Hybridisms.Server.Data;

// TODO: Data - [Z] 
public class HybridismsDbContext(DbContextOptions<HybridismsDbContext> options) : DbContext(options)
{
    public DbSet<NoteEntity> Notes => Set<NoteEntity>();

    public DbSet<TopicEntity> Topics => Set<TopicEntity>();

    public DbSet<NotebookEntity> Notebooks => Set<NotebookEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NoteEntity>().ToTable("Notes");
        modelBuilder.Entity<TopicEntity>().ToTable("Topics");
        modelBuilder.Entity<NotebookEntity>().ToTable("Notebooks");

        modelBuilder.Entity<NoteEntity>()
            .HasMany(n => n.Topics)
            .WithMany(t => t.Notes)
            .UsingEntity(j => j.ToTable("NoteTopics"));

        modelBuilder.Entity<NotebookEntity>()
            .HasMany(nb => nb.Notes)
            .WithOne(n => n.Notebook)
            .HasForeignKey(n => n.NotebookId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
