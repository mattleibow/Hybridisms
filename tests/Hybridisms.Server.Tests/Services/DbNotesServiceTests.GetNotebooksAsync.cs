using Xunit;
using Hybridisms.Server.Data;
using Hybridisms.Server.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hybridisms.Server.Tests.Services;

public partial class DbNotesServiceTests
{
    public class GetNotebooksAsync : DbNotesServiceTestsBase
    {
        [Fact]
        public async Task ReturnsMappedNotebooks()
        {
            Db.Notebooks.Add(new NotebookEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test",
                Description = "Desc"
            });
            Db.SaveChanges();

            var service = new DbNotesService(Db);

            var result = await service.GetNotebooksAsync();

            Assert.Single(result);
            Assert.Equal("Test", result.First().Title);
        }
    }
}
