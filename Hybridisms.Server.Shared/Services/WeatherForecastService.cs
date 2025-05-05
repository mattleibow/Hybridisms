using Hybridisms.Client.Shared;
using Hybridisms.Client.Shared.Services;

namespace Hybridisms.Server.Shared.Services;

public class WeatherForecastService
{
    private const int MinTemperature = -20;
    private const int MaxTemperature = 55;

    private static readonly string[] Summaries =
    [
        "Freezing", "Icy", "Frosty", "Bracing",
        "Chilly", "Cool", "Mild", "Warm",
        "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public async Task<WeatherForecast[]> GetForecastsAsync(int days = 5)
    {
        var startDate = DateOnly.FromDateTime(DateTime.Now);

        await Task.Delay(1000);

        return Enumerable
            .Range(1, days)
            .Select(index =>
            {
                var temp = Random.Shared.Next(MinTemperature, MaxTemperature + 1);
                var summaryIndex = (temp - MinTemperature) * Summaries.Length / (MaxTemperature - MinTemperature + 1);

                return new WeatherForecast
                {
                    Date = startDate.AddDays(index),
                    TemperatureC = temp,
                    Summary = Summaries[summaryIndex]
                };
            })
            .ToArray();
    }
}
