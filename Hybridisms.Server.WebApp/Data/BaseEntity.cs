namespace Hybridisms.Server.WebApp.Data;

public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

    public DateTimeOffset Modified { get; set; } = DateTimeOffset.Now;
}
