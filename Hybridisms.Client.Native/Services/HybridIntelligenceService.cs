using Hybridisms.Shared.Services;
using Microsoft.Extensions.Logging;

namespace Hybridisms.Client.Native.Services;

// TODO: AI - [C] Hybrid AI service
/// <summary>
/// HybridIntelligenceService is a hybrid AI service that combines local and remote AI capabilities.
/// 
/// The service first attempts to use the remote AI service, and if it fails, it falls back to the local AI service.
/// It provides a seamless experience for interacting with AI, regardless of the user's connectivity. 
/// </summary>
public class HybridIntelligenceService(RemoteIntelligenceService remote, EmbeddedIntelligenceService local, RemoteAvailabilityWatcher availability, ILogger<HybridIntelligenceService>? logger) : IIntelligenceService
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

    /// <summary>
    /// Executes a remote function and falls back to a local function if the remote function fails.
    /// </summary>
    private async Task<ICollection<T>> WithLocalFallback<T>(
        Func<CancellationToken, Task<ICollection<T>>> remoteFunc,
        Func<CancellationToken, Task<ICollection<T>>> localFunc,
        CancellationToken cancellationToken = default)
    {
        // If we failed too recently, don't try right away
        if (availability.IsRemoteAvailable)
        {
            logger?.LogWarning("Remote servers were unavailable too recently {TimeAgo}, skipping remote request.", availability.LastUnavailable);

            var results = await localFunc(cancellationToken);
            return results;
        }

        try
        {
            var results = await remoteFunc(cancellationToken);
            return results;
        }
        catch
        {
            availability.MarkRemoteUnavailable();
            logger?.LogWarning("Remote servers are unavailable, falling back to local service.");

            var results = await localFunc(cancellationToken);
            return results;
        }
    }

    /// <summary>
    /// Executes a remote function and falls back to a local function if the remote function fails.
    /// </summary>
    private async Task<T> WithLocalFallback<T>(
        Func<CancellationToken, Task<T>> remoteFunc,
        Func<CancellationToken, Task<T>> localFunc,
        CancellationToken cancellationToken = default)
    {
        // If we failed too recently, don't try right away
        if (availability.IsRemoteAvailable)
        {
            logger?.LogWarning("Remote servers were unavailable too recently {TimeAgo}, skipping remote request.", availability.LastUnavailable);

            var results = await localFunc(cancellationToken);
            return results;
        }

        try
        {
            var results = await remoteFunc(cancellationToken);
            return results;
        }
        catch
        {
            availability.MarkRemoteUnavailable();
            logger?.LogWarning("Remote servers are unavailable, falling back to local service.");

            var results = await localFunc(cancellationToken);
            return results;
        }
    }
}
