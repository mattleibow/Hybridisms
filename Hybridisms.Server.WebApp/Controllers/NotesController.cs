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
        var notes = await noteService.GetStarredNotesAsync();
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
        var savedNotes = await noteService.SaveNotesAsync(notes);
        return Ok(savedNotes);
    }
}
