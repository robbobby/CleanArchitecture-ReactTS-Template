using IdentityModel.Client;

namespace Console.WebUI.Services; 

public interface ITokenService {
    Task<TokenResponse> GetToken(string scope);
}
