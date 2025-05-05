using Hybridisms.Server.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hybridisms.Server.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController(WeatherForecastService forecastService) : ControllerBase
{
    [HttpGet("forecasts")]
    public async Task<IActionResult> GetForecasts()
    {
        var forecasts = await forecastService.GetForecastsAsync();

        if (forecasts?.Length >= 0)
        {
            return Ok(forecasts);
        }

        return NotFound("No forecasts available.");
    }
}
