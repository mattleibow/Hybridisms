namespace Hybridisms.Shared.Services;

public abstract class ModelBase
{
    public Guid Id { get; set; }

    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset Modified { get; set; } = DateTimeOffset.UtcNow;

    private bool isDeleted = false;
    public bool IsDeleted
    {
        get => isDeleted;
        set
        {
            if (isDeleted != value)
            {
                isDeleted = value;
                Modified = DateTime.UtcNow;
            }
        }
    }
}
