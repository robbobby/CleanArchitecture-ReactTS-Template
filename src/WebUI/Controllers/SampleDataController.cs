using System.Collections.Immutable;
using Console.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Console.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SampleDataController : ControllerBase
    {
        public static readonly ImmutableArray<string> Summaries = ImmutableArray.Create(new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        });

        [HttpGet]
        public IEnumerable<WeatherForecast> WeatherForecasts(int startDateIndex)
        {
            var rng = new Random();

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                DateFormatted = DateTime.Now.AddDays(index + startDateIndex).ToString("d")
            })
            .ToArray();
        }
    }
}