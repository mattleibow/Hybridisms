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
        var notebooks = new List<Notebook>();
        await foreach (var notebook in noteService.GetNotebooksAsync())
        {
            notebooks.Add(notebook);
        }
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
        var notes = new List<Note>();
        await foreach (var note in noteService.GetNotesAsync(notebookId))
        {
            notes.Add(note);
        }
        return Ok(notes);
    }
}
