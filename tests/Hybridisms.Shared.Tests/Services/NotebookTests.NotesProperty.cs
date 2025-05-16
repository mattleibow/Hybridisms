using Xunit;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;
public partial class NotebookTests
{
    public class NotesProperty
    {
        [Fact]
        public void Setter_UpdatesNotesAndModified()
        {
            var notebook = new Notebook();
            var oldModified = notebook.Modified;
            var notes = new[] { new Note { Title = "A" } };
            
            notebook.Notes = notes;

            Assert.Equal(notes, notebook.Notes);
            Assert.True(notebook.Modified > oldModified);
        }
    }
}
