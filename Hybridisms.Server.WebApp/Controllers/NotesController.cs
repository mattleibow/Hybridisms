using Hybridisms.Server.Services;
using Hybridisms.Shared.Services;
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

    [HttpDelete("{noteId}")]
    public async Task<IActionResult> DeleteNoteById(Guid noteId)
    {
        await noteService.DeleteNoteAsync(noteId);
        return NoContent();
    }
}
