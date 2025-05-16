using Xunit;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;
public partial class NoteTests
{
    public class TitleProperty
    {
        [Fact]
        public void Setter_UpdatesTitleAndModified()
        {
            var note = new Note();
            var oldModified = note.Modified;
            var newTitle = "Test Title";

            note.Title = newTitle;

            Assert.Equal(newTitle, note.Title);
            Assert.True(note.Modified > oldModified);
        }
    }
}
