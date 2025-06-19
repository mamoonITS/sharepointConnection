using Microsoft.Identity.Client;

namespace dumm1.auth
{
    public class AzureAdAuthService
    {
        private readonly string _clientId;
        private readonly string _tenantId;
        private readonly string[] _scopes;
        private IPublicClientApplication _app;

        public AzureAdAuthService(string clientId, string tenantId, string[] scopes)
        {
            _clientId = clientId;
            _tenantId = tenantId;
            _scopes = scopes;

            _app = PublicClientApplicationBuilder
                .Create(_clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
                .WithRedirectUri("http://localhost") // For desktop apps
                .Build();
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var accounts = await _app.GetAccountsAsync();

            try
            {
                // Try to get token silently
                var result = await _app.AcquireTokenSilent(_scopes, accounts.FirstOrDefault())
                                      .ExecuteAsync();
                return result.AccessToken;
            }
            catch (MsalUiRequiredException)
            {
                // Interactive login required
                var result = await _app.AcquireTokenInteractive(_scopes)
                                      .ExecuteAsync();
                return result.AccessToken;
            }
        }


    }
}
