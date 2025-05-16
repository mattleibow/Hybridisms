using Xunit;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;
public partial class TopicTests
{
    public class ColorProperty
    {
        [Fact]
        public void Setter_UpdatesColorAndModified()
        {
            var topic = new Topic();
            var oldModified = topic.Modified;
            var newColor = "#ff0000";
            topic.Color = newColor;
            Assert.Equal(newColor, topic.Color);
            Assert.True(topic.Modified > oldModified);
        }
    }
}
