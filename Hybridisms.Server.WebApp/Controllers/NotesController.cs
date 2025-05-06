using Hybridisms.Client.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hybridisms.Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController(INotesService noteService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetNotes()
    {
        var notes = new List<Note>();
        await foreach (var note in noteService.GetNotesAsync())
        {
            notes.Add(note);
        }
        
        if (notes.Count >= 0)
        {
            return Ok(notes);
        }

        return NotFound("No notes available.");
    }
}
