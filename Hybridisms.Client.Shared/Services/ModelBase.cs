namespace Hybridisms.Client.Shared.Services;

public abstract class ModelBase
{
    public Guid Id { get; set; }

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset Modified { get; set; }
}
