using Azure.Core;
using Azure.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace dumm1.auth
{
    public class MyTokenCredential : TokenCredential
    {
        private readonly string[] _scopes;
        private readonly InteractiveBrowserCredential _credential;

        public MyTokenCredential(string clientId, string tenantId, string[] scopes)
        {
            _scopes = scopes;
            _credential = new InteractiveBrowserCredential(new InteractiveBrowserCredentialOptions
            {
                ClientId = clientId,
                TenantId = tenantId,
                RedirectUri = new Uri("http://localhost")
            });
        }

        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return GetTokenAsync(requestContext, cancellationToken).GetAwaiter().GetResult();
        }

        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _credential.GetTokenAsync(
                    new TokenRequestContext(_scopes),
                    cancellationToken);

                return token;
            }
            catch (AuthenticationFailedException ex)
            {
                Console.WriteLine($"Authentication failed: {ex.Message}");
                throw;
            }
        }
    }
}