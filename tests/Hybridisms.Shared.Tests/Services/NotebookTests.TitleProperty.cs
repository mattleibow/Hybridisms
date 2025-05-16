using Xunit;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;

public partial class NotebookTests
{
    public class TitleProperty
    {
        [Fact]
        public void Setter_UpdatesTitleAndModified()
        {
            var notebook = new Notebook();
            var oldModified = notebook.Modified;
            var newTitle = "Notebook Title";

            notebook.Title = newTitle;

            Assert.Equal(newTitle, notebook.Title);
            Assert.True(notebook.Modified > oldModified);
        }
    }
}
