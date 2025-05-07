namespace Hybridisms.Server.WebApp.Data;

public class LabelEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;

    public ICollection<NoteEntity> Notes { get; set; } = [];
}
