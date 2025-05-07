using SQLite;

namespace Hybridisms.Client.NativeApp.Data;

[Table("Topics")]
public class TopicEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;

    [Ignore]
    public List<NoteEntity>? Notes { get; set; }
}
