using Hybridisms.Server.Services;
using Hybridisms.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hybridisms.Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotebookController(INotesService noteService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetNotebooks()
    {
        var notebooks = await noteService.GetNotebooksAsync();
        return Ok(notebooks);
    }

    [HttpGet("{notebookId}")]
    public async Task<IActionResult> GetNotebook(Guid notebookId)
    {
        var notebook = await noteService.GetNotebookAsync(notebookId);
        if (notebook is null)
            return NotFound();
        return Ok(notebook);
    }

    [HttpPost("{notebookId}/notes")]
    public async Task<IActionResult> SaveNotes(Guid notebookId, [FromQuery] bool fetchAll, [FromBody] IEnumerable<Note> notes)
    {
        var savedNotes = await noteService.SaveNotebookNotesAsync(notebookId, notes);
        if (!fetchAll)
            return Ok(savedNotes);

        var allNotes = await noteService.GetNotesAsync(notebookId, includeDeleted: true);
        return Ok(allNotes);
    }

    [HttpGet("{notebookId}/notes")]
    public async Task<IActionResult> GetNotes(Guid notebookId, [FromQuery] bool includeDeleted = false)
    {
        var notes = await noteService.GetNotesAsync(notebookId, includeDeleted);
        return Ok(notes);
    }

    [HttpPost]
    public async Task<IActionResult> SaveNotebooks([FromBody] IEnumerable<Notebook> notebooks)
    {
        var savedNotebooks = await noteService.SaveNotebooksAsync(notebooks);
        return Ok(savedNotebooks);
    }
}
