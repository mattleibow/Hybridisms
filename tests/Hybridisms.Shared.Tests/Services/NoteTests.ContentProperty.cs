using Xunit;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;
public partial class NoteTests
{
    public class ContentProperty
    {
        [Fact]
        public void Setter_UpdatesContentAndHtmlContent()
        {
            var note = new Note();
            var content = "# Heading";

            note.Content = content;

            Assert.Equal(content, note.Content);
            Assert.Contains("<h1>", note.HtmlContent);
        }
    }
}
