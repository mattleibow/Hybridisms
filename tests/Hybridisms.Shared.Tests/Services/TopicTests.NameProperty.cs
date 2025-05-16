using Xunit;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;
public partial class TopicTests
{
    public class NameProperty
    {
        [Fact]
        public void Setter_UpdatesNameAndModified()
        {
            var topic = new Topic();
            var oldModified = topic.Modified;
            var newName = "Topic Name";
            topic.Name = newName;
            Assert.Equal(newName, topic.Name);
            Assert.True(topic.Modified > oldModified);
        }
    }
}
