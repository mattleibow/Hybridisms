using System;
using System.Collections.Generic;

namespace Hybridisms.Server.Data;

public class NoteEntity : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public ICollection<TopicEntity> Topics { get; set; } = [];

    public Guid NotebookId { get; set; }

    public NotebookEntity Notebook { get; set; } = null!;

    public bool Starred { get; set; } = false;

    public bool IsDeleted { get; set; } = false;
}
