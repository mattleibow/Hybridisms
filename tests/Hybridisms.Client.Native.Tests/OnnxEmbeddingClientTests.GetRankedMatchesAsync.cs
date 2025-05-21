using Hybridisms.Client.Native.Services;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;

namespace Hybridisms.Client.Native.Tests;

public partial class OnnxEmbeddingClientTests
{
    public class GetRankedMatchesAsync(ITestOutputHelper output) : OnnxEmbeddingClientTestBase(output)
    {
        protected OnnxEmbeddingClient CreateClient([CallerMemberName] string testMethodName = null!) =>
            CreateClient("miniml", testMethodName);

        [Fact]
        public async Task ReturnsEmpty_WhenTextIsNullOrWhitespace()
        {
            var client = CreateClient();

            var result = await client.GetRankedMatchesAsync<string>(" ", ["a", "b"]);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ReturnsEmpty_WhenNoValidOptions()
        {
            var client = CreateClient();

            var result = await client.GetRankedMatchesAsync<string>("test", ["", null!]);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ReturnsRankedResults_ForValidInput()
        {
            var client = CreateClient();

            var optionsList = new List<string> { "apple", "banana", "carrot" };
            var result = await client.GetRankedMatchesAsync("apple", optionsList, 2);

            Assert.True(result.Count <= 2);
            Assert.All(result, r => Assert.Contains(r.Match, optionsList));
        }

        [Theory]
        [InlineData("apple", new[] { "apple", "banana", "carrot" }, new[] { "apple", "banana" })]
        [InlineData("I need to buy chicken and rolls for dinner.", new[] { "Shopping", "Exercise", "Hobbies", "Groceries" }, new[] { "Groceries", "Shopping" })]
        public async Task ReturnsRankedResults_WithCorrectOrder(string input, string[] options, string[] expected)
        {
            var client = CreateClient();

            var result = await client.GetRankedMatchesAsync(input, options, 2);

            Assert.Equal(expected, result.Select(r => r.Match));
        }

        [Fact]
        public async Task RespectsCancellation()
        {
            var client = CreateClient();

            var optionsList = new List<string> { "a", "b", "c" };

            using var cts = new CancellationTokenSource();
            cts.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            {
                await client.GetRankedMatchesAsync("test", optionsList, 3, cts.Token);
            });
        }

        [Fact]
        public async Task ReturnsAll_WhenCountExceedsOptions()
        {
            var client = CreateClient();

            var optionsList = new List<string> { "x", "y" };
            var result = await client.GetRankedMatchesAsync("x", optionsList, 10);

            Assert.Equal(optionsList.Count, result.Count);
        }
    }
}
