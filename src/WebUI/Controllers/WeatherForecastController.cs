using Console.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using Console.WebUI.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Console.WebUI.Controllers {
    [ApiController]
    [Route("[controller]")]
    
    public class WeatherForecastController : ControllerBase {
        private static readonly string[] Summaries = new[] {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITokenService _tokenService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITokenService tokenService) {
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get() {
            var token = await HttpContext.GetTokenAsync("access_token");

            using var client = new HttpClient();
            client.SetBearerToken(token);

            var result = await client.GetAsync("https://localhost:7188/WeatherForecast");
            if (result.IsSuccessStatusCode) {
                var model = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<WeatherForecast>>(model);
                return Ok(data);
            }
            System.Console.WriteLine(result.StatusCode);
            throw new Exception("Unable to get content");
        }
    }
}
