using Hybridisms.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;

namespace Hybridisms.Server.Tests.Worker;

public partial class DbSeedWorkerTests
{
    private static IServiceScopeFactory CreateScopeFactory(HybridismsDbContext dbContext)
    {
        var scope = Substitute.For<IServiceScope>();
        scope.ServiceProvider.GetService(typeof(HybridismsDbContext)).Returns(dbContext);

        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        scopeFactory.CreateScope().Returns(scope);

        return scopeFactory;
    }

    private static HybridismsDbContext CreateInMemoryDb()
    {
        var dbOptions = new DbContextOptionsBuilder<HybridismsDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        var dbContext = new HybridismsDbContext(dbOptions);

        return dbContext;
    }
}
