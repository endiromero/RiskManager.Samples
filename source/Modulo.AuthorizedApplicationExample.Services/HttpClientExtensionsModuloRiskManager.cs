using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Modulo.AuthorizedApplicationExample.Services
{
    public static class HttpClientExtensionsModuloRiskManager
    {
        public static async Task GetAndInstallAnonymousToken(this HttpClient client, ModuloRiskManagerConfig config)
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("grant_type", config.Id);
            dictionary.Add("client_id", config.Id);
            dictionary.Add("client_secret", config.Key);
            var content = new FormUrlEncodedContent(dictionary);
            var response = await client.PostAsync("/APIIntegration/Token", content);
            var token = await response.Content.ReadAsAsync<TokenResponseDto>();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth2", token.access_token);
        }

        public static void GetAndInstallUserContext(this HttpClient client, ModuloRiskManagerConfig config, ModuloRiskManagerUserContext userContext)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("OAuth2", userContext.AccessToken);            
        }
    }
}
