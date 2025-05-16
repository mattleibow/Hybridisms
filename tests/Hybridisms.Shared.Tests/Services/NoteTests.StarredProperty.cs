using Xunit;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;
public partial class NoteTests
{
    public class StarredProperty
    {
        [Fact]
        public void Setter_UpdatesStarred()
        {
            var note = new Note();

            Assert.False(note.Starred);

            note.Starred = true;

            Assert.True(note.Starred);
        }
    }
}
