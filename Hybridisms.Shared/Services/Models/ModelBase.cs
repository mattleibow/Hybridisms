namespace Hybridisms.Shared.Services;

public abstract class ModelBase
{
    public Guid Id { get; set; }

    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset Modified { get; set; } = DateTimeOffset.UtcNow;
}
