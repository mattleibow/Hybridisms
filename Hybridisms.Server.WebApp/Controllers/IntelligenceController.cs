using System.Text;
using Hybridisms.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hybridisms.Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntelligenceController(IIntelligenceService intelligenceService) : ControllerBase
{
    [HttpPost("recommend-topics")]
    public async Task<ActionResult> RecommendTopics([FromQuery] int count, [FromBody] Note note)
    {
        var recommendations = await intelligenceService.RecommendTopicsAsync(note, count);
        return Ok(recommendations);
    }

    [HttpPost("generate-note-contents")]
    public async Task<ActionResult> GenerateNoteContents([FromBody] string prompt)
    {
        var response = await intelligenceService.GenerateNoteContentsAsync(prompt);
        return Ok(response);
    }
}
