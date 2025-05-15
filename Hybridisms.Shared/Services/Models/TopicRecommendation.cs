using System;

namespace Hybridisms.Shared.Services;

public sealed class TopicRecommendation
{
    public required Topic Topic { get; init; }
    public required string Reason { get; init; }
}
