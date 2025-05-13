using System.Text;
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

    [HttpPost("stream-note-contents")]
    public async Task<ActionResult> StreamNoteContents([FromBody] string prompt)
    {
        var sb = new StringBuilder();
        await foreach (var content in intelligenceService.StreamNoteContentsAsync(prompt))
        {
            sb.Append(content);
        }
        return Ok(sb.ToString());
    }
}
