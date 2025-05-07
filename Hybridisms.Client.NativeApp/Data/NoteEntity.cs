using SQLite;

namespace Hybridisms.Client.NativeApp.Data;

[Table("Notes")]
public class NoteEntity : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool Starred { get; set; } = false;

    public Guid NotebookId { get; set; }

    [Ignore]
    public NotebookEntity? Notebook { get; set; }

    [Ignore]
    public List<LabelEntity>? Labels { get; set; }
}
