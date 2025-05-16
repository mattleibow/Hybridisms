using System.Threading;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;
public partial class NotesServiceExtensionsTests
{
    public class SaveNotebookAsyncMethod
    {
        [Fact]
        public async Task CallsSaveNotebooksAsyncAndReturnsNotebook()
        {
            var notesService = Substitute.For<INotesService>();
            var notebook = new Notebook { Title = "Test" };
            notesService.SaveNotebooksAsync(Arg.Any<Notebook[]>(), Arg.Any<CancellationToken>())
                .Returns(new[] { notebook });

            var result = await notesService.SaveNotebookAsync(notebook);

            Assert.Equal(notebook, result);
            await notesService.Received(1).SaveNotebooksAsync(Arg.Is<Notebook[]>(n => n.Length == 1 && n[0] == notebook), Arg.Any<CancellationToken>());
        }
    }
}
