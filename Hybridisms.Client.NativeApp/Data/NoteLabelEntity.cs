using SQLite;

namespace Hybridisms.Client.NativeApp.Data;

[Table("NoteLabels")]
public class NoteLabelEntity
{
    [PrimaryKey]
    public Guid NoteId { get; set; }

    [PrimaryKey]
    public Guid LabelId { get; set; }
}
