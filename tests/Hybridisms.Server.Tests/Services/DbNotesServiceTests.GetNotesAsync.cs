using Xunit;
using Hybridisms.Server.Data;
using Hybridisms.Server.Services;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Hybridisms.Server.Tests.Services;

public partial class DbNotesServiceTests
{
    public class GetNotesAsync : DbNotesServiceTestsBase
    {
        [Fact]
        public async Task ReturnsNotesForNotebook()
        {
            var notebookId = Guid.NewGuid();
            Db.Notes.Add(new NoteEntity
            {
                Id = Guid.NewGuid(),
                Title = "Note1",
                NotebookId = notebookId
            });
            Db.SaveChanges();

            var service = new DbNotesService(Db);

            var result = await service.GetNotesAsync(notebookId);

            Assert.Single(result);
            Assert.Equal("Note1", result.First().Title);
        }
    }
}
