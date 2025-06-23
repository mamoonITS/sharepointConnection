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
        private readonly TokenCredential _credential; // Changed to base type

        public MyTokenCredential(string clientId, string tenantId, string[] scopes, string clientSecret = null)
        {
            _scopes = scopes;
            try
            {
                // In MyTokenCredential constructor, modify to:
                if (!string.IsNullOrEmpty(clientSecret))
                {
                    _credential = new ClientSecretCredential(
                        tenantId,
                        clientId,
                        clientSecret,
                        new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud });
                }
            
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return GetTokenAsync(requestContext, cancellationToken).GetAwaiter().GetResult();
        }

        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            try
            {
                return await _credential.GetTokenAsync(
                    new TokenRequestContext(_scopes),
                    cancellationToken);
            }
            catch (AuthenticationFailedException ex)
            {
                Console.WriteLine($"Authentication failed: {ex.Message}");
                throw;
            }
        }
    }
}