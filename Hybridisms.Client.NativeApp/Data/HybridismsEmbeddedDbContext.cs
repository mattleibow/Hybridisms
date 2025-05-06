using System.Data.Common;
using Microsoft.Extensions.Options;
using SQLite;

namespace Hybridisms.Client.NativeApp.Data;

public class HybridismsEmbeddedDbContext(IOptions<HybridismsEmbeddedDbContext.DbContextOptions> options)
{
    private SQLiteAsyncConnection? connection;

    public string DatabasePath => options.Value.DatabasePath;

    public SQLiteAsyncConnection Connection => connection ??= new(DatabasePath);

    public DbSet<NoteEntity> Notes => new(Connection);

    public async Task EnsureCreatedAsync()
    {
        await Connection.CreateTableAsync<NoteEntity>();
    }

    public class DbSet<T>(SQLiteAsyncConnection connection) : AsyncTableQuery<T>(connection.GetConnection().Table<T>())
        where T : class, new()
    {
        public async Task InsertAllAsync(IEnumerable<T> entities) =>
            await connection.InsertAllAsync(entities);
            
        public async Task DeleteAllAsync() =>
            await connection.DeleteAllAsync<T>();
    }

    public class DbContextOptions
    {
        public required string DatabasePath { get; set; }
    }
}
