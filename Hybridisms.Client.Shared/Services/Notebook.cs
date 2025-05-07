namespace Hybridisms.Client.Shared.Services;

public class Notebook : ModelBase
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<Note> Notes { get; set; } = [];
}