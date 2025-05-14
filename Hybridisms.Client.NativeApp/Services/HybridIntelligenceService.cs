using Hybridisms.Client.Shared.Services;
using Microsoft.Extensions.Logging;

namespace Hybridisms.Client.NativeApp.Services;

public class HybridIntelligenceService(RemoteIntelligenceService remote, EmbeddedIntelligenceService local, ILogger<HybridIntelligenceService>? logger) : IIntelligenceService
{
    public Task<ICollection<TopicRecommendation>> RecommendTopicsAsync(Note note, int count = 3, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Recommending topics for note with ID: {NoteId}", note.Id);

        return WithLocalFallback(
            ct => remote.RecommendTopicsAsync(note, count, ct),
            ct => local.RecommendTopicsAsync(note, count, ct),
            cancellationToken);
    }

    public Task<string> GenerateNoteContentsAsync(string prompt, CancellationToken cancellationToken = default)
    {
        logger?.LogInformation("Streaming note contents with prompt: {Prompt}", prompt);
        
        return WithLocalFallback(
            ct => remote.GenerateNoteContentsAsync(prompt, ct),
            ct => local.GenerateNoteContentsAsync(prompt, ct),
            cancellationToken);
    }

    private async Task<ICollection<T>> WithLocalFallback<T>(
        Func<CancellationToken, Task<ICollection<T>>> remoteFunc,
        Func<CancellationToken, Task<ICollection<T>>> localFunc,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var results = await remoteFunc(cancellationToken);
            return results;
        }
        catch
        {
            var results = await localFunc(cancellationToken);
            return results;
        }
    }
    
    private async Task<T> WithLocalFallback<T>(
        Func<CancellationToken, Task<T>> remoteFunc,
        Func<CancellationToken, Task<T>> localFunc,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var results = await remoteFunc(cancellationToken);
            return results;
        }
        catch
        {
            var results = await localFunc(cancellationToken);
            return results;
        }
    }
}
