using Hybridisms.Client.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hybridisms.Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationController(ITopicRecommendationService topicRecommendations) : ControllerBase
{
    [HttpPost("topics")]
    public async Task<ActionResult> RecommendTopics([FromQuery] int count, [FromBody] Note note)
    {
        var topics = new List<Topic>();
        await foreach (var topic in topicRecommendations.RecommendTopicsAsync(note, count))
        {
            topics.Add(topic);
        }
        return Ok(topics);
    }
}
