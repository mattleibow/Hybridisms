using Xunit;
using Hybridisms.Server.Services;
using Hybridisms.Shared.Services;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Hybridisms.Server.Tests.Services;

public partial class DbNotesServiceTests
{
    public class SaveNotebooksAsync : DbNotesServiceTestsBase
    {
        [Fact]
        public async Task CreatesNewNotebook()
        {
            var service = new DbNotesService(Db);
            var notebook = new Notebook
            {
                Id = Guid.NewGuid(),
                Title = "New",
                Description = "Desc",
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };

            var result = await service.SaveNotebooksAsync([notebook]);

            Assert.Single(result);
            Assert.Equal("New", Db.Notebooks.First().Title);
        }
    }
}
