namespace Console.WebUI.Models; 

public class AuthUser : IAuthUser {

    public AuthUser(string status, string token, string userName) {
        Status = status;
        Token = token;
        UserName = userName;
    }

    public string Status { get; }
    public string Token { get; }
    public string UserName { get; }
}
