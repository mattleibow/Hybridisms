using SQLite;

namespace Hybridisms.Client.NativeApp.Data;

[Table("Labels")]
public class LabelEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;

    [Ignore]
    public List<NoteEntity>? Notes { get; set; }
}
