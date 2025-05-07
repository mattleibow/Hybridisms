using Markdig;

namespace Hybridisms.Client.Shared.Services;

public class Note : ModelBase
{
    public string Title { get; set; } = string.Empty;

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
            }
        }
    }

    public bool Starred { get; set; } = false;

    public List<Topic> Topics { get; set; } = [];

    public Guid NotebookId { get; set; }

    private string? htmlContent;
    public string HtmlContent =>
        htmlContent ??= Markdown.ToHtml(content ?? string.Empty);
}
