using Hybridisms.Client.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hybridisms.Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController(INotesService noteService) : ControllerBase
{
    [HttpGet("starred")]
    public async Task<IActionResult> GetStarredNotes()
    {
        var notes = new List<Note>();
        await foreach (var note in noteService.GetStarredNotesAsync())
        {
            notes.Add(note);
        }
        return Ok(notes);
    }

    [HttpGet("{noteId}")]
    public async Task<IActionResult> GetNoteById(Guid noteId)
    {
        var note = await noteService.GetNoteAsync(noteId);
        if (note == null)
            return NotFound();
        return Ok(note);
    }

    [HttpPost]
    public async Task<IActionResult> SaveNotes([FromBody] IEnumerable<Note> notes)
    {
        var savedNotes = new List<Note>();
        await foreach (var note in noteService.SaveNotesAsync(notes))
        {
            savedNotes.Add(note);
        }
        return Ok(savedNotes);
    }
}
