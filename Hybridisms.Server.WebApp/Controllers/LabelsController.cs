using Hybridisms.Client.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hybridisms.Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabelsController(INotesService noteService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLabels()
    {
        var labels = new List<Label>();
        await foreach (var label in noteService.GetLabelsAsync())
        {
            labels.Add(label);
        }
        return Ok(labels);
    }
}
