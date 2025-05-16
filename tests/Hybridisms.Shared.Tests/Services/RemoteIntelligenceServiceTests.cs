using System.Threading;
using System.Threading.Tasks;

namespace Hybridisms.Shared.Tests.Services;
public partial class RemoteIntelligenceServiceTests
{
    // Minimal fake HttpClient for demonstration (replace with a proper mock or handler for real tests)
    public class TestHttpClient : System.Net.Http.HttpClient
    {
        public override async Task<System.Net.Http.HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new System.Net.Http.StringContent("[]")
            };
            return await Task.FromResult(response);
        }
    }
}
