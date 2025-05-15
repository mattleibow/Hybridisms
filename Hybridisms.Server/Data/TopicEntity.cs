using System;

namespace Hybridisms.Server.Data;

public class TopicEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = string.Empty;

    public ICollection<NoteEntity> Notes { get; set; } = [];
}
