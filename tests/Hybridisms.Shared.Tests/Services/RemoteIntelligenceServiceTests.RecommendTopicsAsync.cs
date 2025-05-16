using System.Threading.Tasks;
using Xunit;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;
public partial class RemoteIntelligenceServiceTests
{
    public class RecommendTopicsAsync
    {
        [Fact]
        public async Task ReturnsRecommendations()
        {
            var httpClient = new TestHttpClient();
            var service = new RemoteIntelligenceService(httpClient);
            var note = new Note { Title = "Test" };
            var result = await service.RecommendTopicsAsync(note);
            Assert.NotNull(result);
        }
    }
}
