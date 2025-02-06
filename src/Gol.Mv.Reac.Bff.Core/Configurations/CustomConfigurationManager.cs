using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect
{
    public class CustomConfigurationManager : IConfigurationManager<OpenIdConnectConfiguration>
    {
        private readonly string _discoveryEndpoint;
        private bool _shouldQuery = false;
        private OpenIdConnectConfiguration _openIdConnectConfiguration = new OpenIdConnectConfiguration();

        public CustomConfigurationManager(string authorityUrl)
        {
            if (!authorityUrl.EndsWith('/'))
            {
                authorityUrl += "/";
            }
            _discoveryEndpoint = $"{authorityUrl}.well-known/openid-configuration";
            _shouldQuery = true;
        }

        protected class DiscoveryResult
        {
            [JsonPropertyName("jwks_uri")]
            public string JwksUri { get; set; } = string.Empty;

            [JsonPropertyName("issuer")]
            public string Issuer { get; set; } = string.Empty;
        }

        public async Task<OpenIdConnectConfiguration> GetConfigurationAsync(CancellationToken cancel)
        {
            if (_openIdConnectConfiguration == null || _shouldQuery)
            {
                _openIdConnectConfiguration = await GetOpenIdConnectConfigurationAsync(cancel);
            }

            return _openIdConnectConfiguration;
        }

        public void RequestRefresh()
        {
            _shouldQuery = true;
        }

        private async Task<OpenIdConnectConfiguration> GetOpenIdConnectConfigurationAsync(CancellationToken cancellationToken)
        {
            using HttpClient httpClient = new HttpClient();

            var result = await httpClient.GetFromJsonAsync<DiscoveryResult>(_discoveryEndpoint, cancellationToken);

            if (result == null)
            {
                throw new Exception("Discovery result is null");
            }
            if (string.IsNullOrEmpty(result.JwksUri))
            {
                throw new Exception("JwksUri is null");
            }

            string jsonWebKeySetString = await httpClient.GetStringAsync(result.JwksUri, cancellationToken);

            if (string.IsNullOrEmpty(jsonWebKeySetString))
            {
                throw new Exception("Empty jsonWebKeySet");
            }

            JsonWebKeySet jsonWebKeySet = new JsonWebKeySet(jsonWebKeySetString);

            OpenIdConnectConfiguration openIdConnectConfiguration = new OpenIdConnectConfiguration()
            {
                JsonWebKeySet = jsonWebKeySet,
                Issuer = result.Issuer
            };

            foreach (SecurityKey key in openIdConnectConfiguration.JsonWebKeySet.GetSigningKeys())
            {
                openIdConnectConfiguration.SigningKeys.Add(key);
            }

            return openIdConnectConfiguration;
        }
    }
}