using System.Threading.Tasks;
using Xunit;
using Hybridisms.Shared.Services;

namespace Hybridisms.Shared.Tests.Services;
public partial class RemoteIntelligenceServiceTests
{
    public class GenerateNoteContentsAsync
    {
        [Fact]
        public async Task ReturnsContent()
        {
            var httpClient = new TestHttpClient();
            var service = new RemoteIntelligenceService(httpClient);
            var result = await service.GenerateNoteContentsAsync("prompt");
            Assert.NotNull(result);
        }
    }
}
