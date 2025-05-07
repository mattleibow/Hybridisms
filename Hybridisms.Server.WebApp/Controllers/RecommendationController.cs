using Hybridisms.Client.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hybridisms.Server.WebApp.Controllers;

[ApiController]
[Route("api/recommendation")]
public class RecommendationController(ILabelRecommendationService labelRecommendations) : ControllerBase
{
    [HttpPost("labels")]
    public async Task<ActionResult> RecommendLabels([FromQuery] int count, [FromBody] Note note)
    {
        var labels = new List<Label>();
        await foreach (var label in labelRecommendations.RecommendLabelsAsync(note, count))
        {
            labels.Add(label);
        }
        return Ok(labels);
    }
}
