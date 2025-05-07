using SQLite;

namespace Hybridisms.Client.NativeApp.Data;

[Table("NoteTopics")]
public class NoteTopicEntity
{
    [PrimaryKey]
    public Guid NoteId { get; set; }

    [PrimaryKey]
    public Guid TopicId { get; set; }
}
