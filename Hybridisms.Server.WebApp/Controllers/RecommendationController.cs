using Hybridisms.Client.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hybridisms.Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntelligenceController(IIntelligenceService intelligenceService) : ControllerBase
{
    [HttpPost("recommend-topics")]
    public async Task<ActionResult> RecommendTopics([FromQuery] int count, [FromBody] Note note)
    {
        var recommendations = new List<TopicRecommendation>();
        await foreach (var recommendation in intelligenceService.RecommendTopicsAsync(note, count))
        {
            recommendations.Add(recommendation);
        }
        return Ok(recommendations);
    }
}
