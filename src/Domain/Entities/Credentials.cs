using Console.WebUI.Models;

namespace Console.Domain.Entities;

public class Credentials : ICredentials {
    public string? userName { get; set; }
    public string? password { get; set; }
    public bool rememberMe { get; set; }
}
