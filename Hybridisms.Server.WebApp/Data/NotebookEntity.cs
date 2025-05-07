namespace Hybridisms.Server.WebApp.Data;

public class NotebookEntity : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ICollection<NoteEntity> Notes { get; set; } = [];
}
