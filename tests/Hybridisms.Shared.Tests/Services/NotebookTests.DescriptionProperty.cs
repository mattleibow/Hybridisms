using Xunit;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;

public partial class NotebookTests
{
    public class DescriptionProperty
    {
        [Fact]
        public void Setter_UpdatesDescriptionAndModified()
        {
            var notebook = new Notebook();
            var oldModified = notebook.Modified;
            var newDescription = "Description here";

            notebook.Description = newDescription;

            Assert.Equal(newDescription, notebook.Description);
            Assert.True(notebook.Modified > oldModified);
        }
    }
}
