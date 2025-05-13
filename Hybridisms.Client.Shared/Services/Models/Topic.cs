namespace Hybridisms.Client.Shared.Services;

public class Topic : ModelBase
{
    private string name = string.Empty;
    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                Modified = DateTime.UtcNow;
            }
        }
    }

    private string color = string.Empty;
    public string Color
    {
        get => color;
        set
        {
            if (color != value)
            {
                color = value;
                Modified = DateTime.UtcNow;
            }
        }
    }
}
