using Hybridisms.Client.Shared.Services;
using SQLite;

namespace Hybridisms.Client.NativeApp.Data;

[Table("Notes")]
public class NoteEntity
{
    [PrimaryKey]
    public Guid Id { get; set; }

    public string? Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }

    public Note ToNote() =>
        new()
        {
            Id = Id,
            Date = DateOnly.Parse(Date ?? string.Empty),
            TemperatureC = TemperatureC,
            Summary = Summary
        };

    public static NoteEntity FromNote(Note note) =>
        new()
        {
            Id = note.Id,
            Date = note.Date.ToString("yyyy-MM-dd"),
            TemperatureC = note.TemperatureC,
            Summary = note.Summary
        };
}
