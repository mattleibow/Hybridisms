using SQLite;

namespace Hybridisms.Client.NativeApp.Data;

[Table("NoteTopics")]
public class NoteTopicEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed(Name = "CompositeKey", Order = 1, Unique = true)]
    public Guid NoteId { get; set; }

    [Indexed(Name = "CompositeKey", Order = 2, Unique = true)]
    public Guid TopicId { get; set; }
}
