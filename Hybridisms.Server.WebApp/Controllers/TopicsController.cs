using Hybridisms.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hybridisms.Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicsController(INotesService noteService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTopics()
    {
        var topics = await noteService.GetTopicsAsync();
        return Ok(topics);
    }

    [HttpPost]
    public async Task<IActionResult> SaveTopics([FromBody] IEnumerable<Topic> topics)
    {
        var saved = await noteService.SaveTopicsAsync(topics);
        return Ok(saved);
    }
}
