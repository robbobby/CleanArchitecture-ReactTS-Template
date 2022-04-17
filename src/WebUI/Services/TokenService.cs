using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Console.WebUI.Services; 

public class TokenService : ITokenService {
    private readonly ILogger<TokenService> _logger;
    private readonly IOptions<IdentityServerSettings> _identityServerSettings;
    private readonly DiscoveryDocumentResponse _discoveryDocument;

    public TokenService(ILogger<TokenService> logger, IOptions<IdentityServerSettings> identityServerSettings) {
        System.Console.WriteLine("************************************************************************************************************************");
        System.Console.WriteLine("Making Token Service");
        System.Console.WriteLine("************************************************************************************************************************");
        _logger = logger;
        _identityServerSettings = identityServerSettings;

        using var client = new HttpClient();
        _discoveryDocument = client.GetDiscoveryDocumentAsync(identityServerSettings.Value.DiscoveryUrl).Result;
        if (_discoveryDocument.IsError) {
            logger.LogError($"Unable to get discover document. Error is {_discoveryDocument.Error}");
            throw new Exception("Unable to get discovery document", _discoveryDocument.Exception);
        }
    }
    
    public async Task<TokenResponse> GetToken(string scope) {
        using var client = new HttpClient();
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest() {
            Address = $"{_identityServerSettings.Value.DiscoveryUrl}/connect/token",
            ClientId = _identityServerSettings.Value.ClientName,
            ClientSecret = _identityServerSettings.Value.ClientPassword,
            Scope = scope
        });
        if (tokenResponse.IsError) {
            _logger.LogError($"Unable to get token. Error is {tokenResponse.Error}");
            throw new Exception("Unable to get token ", tokenResponse.Exception);
        }
        return tokenResponse;
    }
}
