using System.Runtime.CompilerServices;
using Hybridisms.Client.Shared.Services;

namespace Hybridisms.Client.NativeApp.Services;

public class HybridIntelligenceService(RemoteIntelligenceService remote, EmbeddedIntelligenceService local) : IIntelligenceService
{
    public IAsyncEnumerable<TopicRecommendation> RecommendTopicsAsync(Note note, int count = 3, [EnumeratorCancellation] CancellationToken cancellationToken = default) =>
        WithLocalFallback(
            ct => remote.RecommendTopicsAsync(note, count, ct),
            ct => local.RecommendTopicsAsync(note, count, ct),
            cancellationToken);

    public IAsyncEnumerable<string> StreamNoteContentsAsync(string prompt, [EnumeratorCancellation] CancellationToken cancellationToken = default) =>
        WithLocalFallback(
            ct => remote.StreamNoteContentsAsync(prompt, ct),
            ct => local.StreamNoteContentsAsync(prompt, ct),
            cancellationToken);

    private async IAsyncEnumerable<T> WithLocalFallback<T>(
        Func<CancellationToken, IAsyncEnumerable<T>> remoteFunc,
        Func<CancellationToken, IAsyncEnumerable<T>> localFunc,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var results = new List<T>();
        try
        {
            await foreach (var item in remoteFunc(cancellationToken).WithCancellation(cancellationToken))
            {
                results.Add(item);
            }
        }
        catch
        {
            results.Clear();
            await foreach (var item in localFunc(cancellationToken).WithCancellation(cancellationToken))
            {
                results.Add(item);
            }
        }
        foreach (var item in results)
        {
            yield return item;
        }
    }
}
