using Xunit;
using Hybridisms.Server.Services;
using System;
using System.Threading.Tasks;

namespace Hybridisms.Server.Tests.Services;
public partial class DbNotesServiceTests
{
    public class GetNotebookAsync : DbNotesServiceTestsBase
    {
        [Fact]
        public async Task ReturnsNullIfNotFound()
        {
            var service = new DbNotesService(Db);

            var result = await service.GetNotebookAsync(Guid.NewGuid());

            Assert.Null(result);
        }
    }
}
