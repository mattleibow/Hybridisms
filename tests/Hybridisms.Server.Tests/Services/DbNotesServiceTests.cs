
using Hybridisms.Server.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hybridisms.Server.Tests.Services;

public partial class DbNotesServiceTests
{
    public abstract class DbNotesServiceTestsBase : IDisposable
    {
        protected readonly HybridismsDbContext Db;

        protected DbNotesServiceTestsBase()
        {
            var options = new DbContextOptionsBuilder<HybridismsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            Db = new HybridismsDbContext(options);
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}
