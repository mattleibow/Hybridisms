using System.ComponentModel.DataAnnotations;

namespace Hybridisms.Server.WebApp.Data;

public class NoteEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}
