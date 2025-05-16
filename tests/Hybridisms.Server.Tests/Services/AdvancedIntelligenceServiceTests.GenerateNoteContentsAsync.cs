using Xunit;
using NSubstitute;
using Hybridisms.Shared.Services;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.AI;
using Hybridisms.Server.Services;

namespace Hybridisms.Server.Tests.Services;

public partial class AdvancedIntelligenceServiceTests
{
    public class GenerateNoteContentsAsync
    {
        [Fact]
        public async Task ReturnsChatClientResponse()
        {
            var notesService = Substitute.For<INotesService>();
            var chatClient = Substitute.For<IChatClient>();
            chatClient.GetResponseAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int?>(),
                Arg.Any<CancellationToken>())
                .Returns(Task.FromResult("AI generated note"));
            var service = new AdvancedIntelligenceService(notesService, chatClient);

            var result = await service.GenerateNoteContentsAsync("Prompt");

            Assert.Equal("AI generated note", result);
        }
    }
}
