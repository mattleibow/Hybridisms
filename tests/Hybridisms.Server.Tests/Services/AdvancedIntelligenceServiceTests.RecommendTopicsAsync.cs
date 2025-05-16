using Xunit;
using NSubstitute;
using Hybridisms.Shared.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.AI;
using System.Linq;
using Hybridisms.Server.Services;

namespace Hybridisms.Server.Tests.Services;

public partial class AdvancedIntelligenceServiceTests
{
    public class RecommendTopicsAsync
    {
        [Fact]
        public async Task ReturnsEmptyList_WhenNoTopics()
        {
            var notesService = Substitute.For<INotesService>();
            notesService
                .GetTopicsAsync(Arg.Any<CancellationToken>())
                .Returns(new List<Topic>());

            var chatClient = Substitute.For<IChatClient>();

            var service = new AdvancedIntelligenceService(notesService, chatClient);

            var note = new Note
            {
                Title = "Test",
                Content = "Test content"
            };
            var result = await service.RecommendTopicsAsync(note, 3);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ReturnsRecommendations_WhenTopicsAndChatClientResponds()
        {
            var topics = new List<Topic>
            {
                new Topic { Name = "C#" },
                new Topic { Name = "AI" }
            };

            var notesService = Substitute.For<INotesService>();
            notesService
                .GetTopicsAsync(Arg.Any<CancellationToken>())
                .Returns(topics);

            var chatClient = Substitute.For<IChatClient>();
            chatClient
                .GetResponseAsync(Arg.Any<IEnumerable<ChatMessage>>(), Arg.Any<ChatOptions?>(), Arg.Any<CancellationToken>())
                .Returns(new ChatResponse(new ChatMessage(ChatRole.Assistant,
                    """
                    [
                      {
                        "label": "C#",
                        "reason": "Because it's about C#"
                      }
                    ]
                    """)));

            var service = new AdvancedIntelligenceService(notesService, chatClient);

            var note = new Note
            {
                Title = "C# basics",
                Content = "Learn C#"
            };

            var result = await service.RecommendTopicsAsync(note, 1);

            Assert.Single(result);
            Assert.Equal("C#", result.First().Topic.Name);
            Assert.Equal("Because it's about C#", result.First().Reason);
        }

        [Fact]
        public async Task IgnoresUnknownTopicsFromAI()
        {
            var topics = new List<Topic>
            {
                new Topic { Name = "C#" }
            };

            var notesService = Substitute.For<INotesService>();
            notesService
                .GetTopicsAsync(Arg.Any<CancellationToken>())
                .Returns(topics);

            var chatClient = Substitute.For<IChatClient>();
            chatClient
                .GetResponseAsync(Arg.Any<IEnumerable<ChatMessage>>(), Arg.Any<ChatOptions?>(), Arg.Any<CancellationToken>())
                .Returns(new ChatResponse(new ChatMessage(ChatRole.Assistant,
                    """
                    [
                      {
                        "label": "Python",
                        "reason": "Because it's about Python"
                      }
                    ]
                    """)));


            var service = new AdvancedIntelligenceService(notesService, chatClient);

            var note = new Note { Title = "Python basics", Content = "Learn Python" };

            var result = await service.RecommendTopicsAsync(note, 1);

            Assert.Empty(result);
        }
    }
}
