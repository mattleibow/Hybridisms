using System.Data.Common;
using Microsoft.Extensions.Options;
using SQLite;

namespace Hybridisms.Client.Native.Data;

public class HybridismsEmbeddedDbContext(IOptions<HybridismsEmbeddedDbContext.DbContextOptions> options)
{
    private SQLiteAsyncConnection? connection;

    public string DatabasePath => options.Value.DatabasePath;

    public SQLiteAsyncConnection Connection => connection ??= new(DatabasePath);

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
