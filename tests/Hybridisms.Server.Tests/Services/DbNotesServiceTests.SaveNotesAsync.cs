using Xunit;
using Hybridisms.Server.Services;
using Hybridisms.Shared.Services;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Hybridisms.Server.Tests.Services;

public partial class DbNotesServiceTests
{
    public class SaveNotesAsync : DbNotesServiceTestsBase
    {
        [Fact]
        public async Task CreatesNewNote()
        {
            var service = new DbNotesService(Db);
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Note",
                Content = "Content",
                NotebookId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow
            };

            var result = await service.SaveNotesAsync([note]);

            Assert.Single(result);
            Assert.Equal("Note", Db.Notes.First().Title);
        }
    }
}
