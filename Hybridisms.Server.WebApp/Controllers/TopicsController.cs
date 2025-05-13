using Hybridisms.Client.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hybridisms.Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicsController(INotesService noteService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTopics()
    {
        var topics = new List<Topic>();
        await foreach (var topic in noteService.GetTopicsAsync())
        {
            topics.Add(topic);
        }
        return Ok(topics);
    }

    [HttpPost]
    public async Task<IActionResult> SaveTopics([FromBody] IEnumerable<Topic> topics)
    {
        var saved = new List<Topic>();
        await foreach (var topic in noteService.SaveTopicsAsync(topics))
        {
            saved.Add(topic);
        }
        return Ok(saved);
    }
}
