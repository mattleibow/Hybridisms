using Markdig;

namespace Hybridisms.Shared.Services;

public class Note : ModelBase
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

    private string content = string.Empty;
    public string Content
    {
        get => content;
        set
        {
            if (content != value)
            {
                content = value;
                htmlContent = null;
                Modified = DateTime.UtcNow;
            }
        }
    }

    private bool starred = false;
    public bool Starred
    {
        get => starred;
        set
        {
            if (starred != value)
            {
                starred = value;
                Modified = DateTime.UtcNow;
            }
        }
    }

    private IReadOnlyList<Topic> topics = [];
    public IReadOnlyList<Topic> Topics
    {
        get => topics;
        set
        {
            if (!ReferenceEquals(topics, value))
            {
                topics = value;
                Modified = DateTime.UtcNow;
            }
        }
    }

    public Guid NotebookId { get; set; }

    private string? htmlContent;
    public string HtmlContent =>
        htmlContent ??= Markdown.ToHtml(content ?? string.Empty);
}
