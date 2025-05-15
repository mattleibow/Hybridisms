using System;

namespace Hybridisms.Server.Web.Data;

public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset Modified { get; set; } = DateTimeOffset.UtcNow;
}
