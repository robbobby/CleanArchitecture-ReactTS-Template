// using Console.Domain.Entities;
// using Console.Infrastructure;
// using Console.WebUI.Hubs;
// using Console.WebUI.Models;
// using Console.WebUI.Services;
// using IdentityModel.Client;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.SignalR;
// using Newtonsoft.Json;
//
// namespace Console.WebUI.Controllers;
//
// [ApiController]
// [Route("api/[controller]/[action]")]
// [AllowAnonymous]
// public class AuthController : ControllerBase {
//     private readonly IHubContext<UsersHub> _hubContext;
//     private readonly ILogger<AuthController> _logger;
//     private readonly IDontCare _dontCare;
//     private readonly ITokenService _tokenService;
//
//     public AuthController(IHubContext<UsersHub> hubContext, ILogger<AuthController> logger, ITokenService tokenService) {
//         _hubContext = hubContext;
//         _logger = logger;
//         _tokenService = tokenService;
//     }
//
//     [HttpGet]
//     [Authorize]
//     public async Task<IActionResult> Login() {
//         var token = await HttpContext.GetTokenAsync("access_token");
//
//         using var client = new HttpClient();
//         client.SetBearerToken(token);
//
//         var result = await client.GetAsync("https://localhost:7188/WeatherForecast");
//         if (result.IsSuccessStatusCode) {
//             var model = await result.Content.ReadAsStringAsync();
//             var data = JsonConvert.DeserializeObject<List<WeatherForecast>>(model);
//             return Ok(data);
//         }
//         System.Console.WriteLine(result.StatusCode);
//         throw new Exception("Unable to get content");
//     }
//
//     // [HttpPost]
//     // [AllowAnonymous]
//     // [ProducesResponseType(typeof(AuthUser), StatusCodes.Status200OK)]
//     // public async Task<IActionResult> Login([FromBody] Credentials request) {
//     //     await _hubContext.Clients.All.SendAsync("UserLogin");
//     //     AuthUser authUser = new("success", "38595847A485DJSHND94857", request?.userName ?? "");
//     //     return Ok(authUser);
//     // }
//     //
//     // [HttpPost]
//     // [ProducesResponseType(StatusCodes.Status200OK)]
//     // public async Task<IActionResult> Logout() {
//     //     await _hubContext.Clients.All.SendAsync("UserLogout");
//     //     return Ok();
//     // }
// }
