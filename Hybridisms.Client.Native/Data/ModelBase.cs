using SQLite;

namespace Hybridisms.Client.Native.Data;

public abstract class BaseEntity
{
    [PrimaryKey]
    public Guid Id { get; set; }

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset Modified { get; set; }
}
