using Xunit;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;
public partial class TopicRecommendationTests
{
    public class Creation
    {
        [Fact]
        public void CanCreateTopicRecommendation()
        {
            var topic = new Topic { Name = "Test" };
            var recommendation = new TopicRecommendation { Topic = topic, Reason = "Relevant" };
            Assert.Equal(topic, recommendation.Topic);
            Assert.Equal("Relevant", recommendation.Reason);
        }
    }
}
