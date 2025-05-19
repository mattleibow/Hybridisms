using Microsoft.Extensions.Options;
using SQLite;

namespace Hybridisms.Client.Native.Data;

// TODO: Data - Embedded/local database
/// <summary>
/// HybridismsEmbeddedDbContext is a lightweight SQLite database context for the Hybridisms application.
/// It provides access to the database and manages the creation of tables.
/// </summary>
public class HybridismsEmbeddedDbContext(IOptions<HybridismsEmbeddedDbContext.DbContextOptions> options)
{
    private SQLiteAsyncConnection? connection;

    public string DatabasePath => options.Value.DatabasePath;

    public SQLiteAsyncConnection Connection => connection ??= CreateConnection();

    public DbSet<NoteEntity> Notes => new(Connection);

    public DbSet<TopicEntity> Topics => new(Connection);

    public DbSet<NotebookEntity> Notebooks => new(Connection);

    public DbSet<NoteTopicEntity> NoteTopics => new(Connection);

    public async Task EnsureCreatedAsync()
    {
        await Connection.CreateTableAsync<NoteEntity>();
        await Connection.CreateTableAsync<TopicEntity>();
        await Connection.CreateTableAsync<NotebookEntity>();
        await Connection.CreateTableAsync<NoteTopicEntity>();
    }

    private SQLiteAsyncConnection CreateConnection()
    {
        if (Path.GetDirectoryName(DatabasePath) is string dir && !string.IsNullOrEmpty(dir))
        {
            Directory.CreateDirectory(dir);
        }

        return new(DatabasePath);
    }

    public class DbSet<T>(SQLiteAsyncConnection connection) : AsyncTableQuery<T>(connection.GetConnection().Table<T>())
        where T : class, new()
    {
        public Task InsertAllAsync(IEnumerable<T> entities) =>
            connection.InsertAllAsync(entities);

        public Task InsertOrReplaceAllAsync(IEnumerable<T> entities) =>
            connection.RunInTransactionAsync(conn =>
            {
                foreach (var entity in entities)
                {
                    conn.InsertOrReplace(entity);
                }
            });

        public Task DeleteAllAsync() =>
            connection.DeleteAllAsync<T>();
    }

    public class DbContextOptions
    {
        public required string DatabasePath { get; set; }
    }
}
