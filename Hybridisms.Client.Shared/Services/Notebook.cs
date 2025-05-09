namespace Hybridisms.Client.Shared.Services;

public class Notebook : ModelBase
{
    private string title = string.Empty;
    public string Title
    {
        get => title;
        set
        {
            if (title != value)
            {
                title = value;
                Modified = DateTime.UtcNow;
            }
        }
    }

    private string description = string.Empty;
    public string Description
    {
        get => description;
        set
        {
            if (description != value)
            {
                description = value;
                Modified = DateTime.UtcNow;
            }
        }
    }

    private IReadOnlyList<Note> notes = [];
    public IReadOnlyList<Note> Notes
    {
        get => notes;
        set
        {
            if (!ReferenceEquals(notes, value))
            {
                notes = value;
                Modified = DateTime.UtcNow;
            }
        }
    }
}