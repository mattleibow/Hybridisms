using SQLite;

namespace Hybridisms.Client.Native.Data;

[Table("Topics")]
public class TopicEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;
}
