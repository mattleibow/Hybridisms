using Hybridisms.Client.Shared.Services;
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

    [HttpGet("{notebookId}/notes")]
    public async Task<IActionResult> GetNotes(Guid notebookId)
    {
        var notes = await noteService.GetNotesAsync(notebookId);
        return Ok(notes);
    }

    [HttpPost]
    public async Task<IActionResult> SaveNotebooks([FromBody] IEnumerable<Notebook> notebooks)
    {
        var savedNotebooks = await noteService.SaveNotebooksAsync(notebooks);
        return Ok(savedNotebooks);
    }
}
