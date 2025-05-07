using SQLite;

namespace Hybridisms.Client.NativeApp.Data;

[Table("Notebooks")]
public class NotebookEntity : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Ignore]
    public List<NoteEntity>? Notes { get; set; }
}
