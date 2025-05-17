using Hybridisms.Server.Data;
using Hybridisms.Server.Worker;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hybridisms.Server.Tests.Worker;

public partial class DbSeedWorkerTests
{
    public class ExecuteAsync
    {
        [Fact]
        public async Task CreatesDatabaseAndSeeds_WhenNoNotebooksExist()
        {
            var dbContext = CreateInMemoryDb();

            var scopeFactory = CreateScopeFactory(dbContext);
            var lifetime = Substitute.For<IHostApplicationLifetime>();
            var worker = new DbSeedWorker(scopeFactory, lifetime);

            await worker.StartAsync(CancellationToken.None);

            Assert.True(await dbContext.Database.EnsureCreatedAsync());
            Assert.NotEmpty(dbContext.Notebooks);
            Assert.NotEmpty(dbContext.Topics);

            lifetime.Received().StopApplication();
        }

        [Fact]
        public async Task DoesNotSeed_WhenNotebooksExist()
        {
            var dbContext = CreateInMemoryDb();
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Notebooks.Add(new NotebookEntity { Id = Guid.NewGuid(), Title = "Existing", Description = "desc" });
            dbContext.SaveChanges();

            var scopeFactory = CreateScopeFactory(dbContext);
            var lifetime = Substitute.For<IHostApplicationLifetime>();
            var worker = new DbSeedWorker(scopeFactory, lifetime);

            await worker.StartAsync(CancellationToken.None);

            Assert.Single(dbContext.Notebooks);
            lifetime.Received().StopApplication();
        }

        [Fact]
        public async Task LogsAndThrows_OnException()
        {
            var scopeFactory = CreateScopeFactory(null!);
            var lifetime = Substitute.For<IHostApplicationLifetime>();
            var worker = new DbSeedWorker(scopeFactory, lifetime);

            await worker.StartAsync(CancellationToken.None);

            lifetime.Received().StopApplication();
        }
    }
}
