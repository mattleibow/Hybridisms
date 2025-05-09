using System;

namespace Hybridisms.Client.Shared.Services;

public sealed class TopicRecommendation
{
    public required Topic Topic { get; init; }
    public required string Reason { get; init; }
}
