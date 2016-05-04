using System;
using System.Net.Http;

namespace Modulo.AuthorizedApplicationExample.Services
{
    public class CallModuloRiskManagerAnonymously
    {
        public class Parameters
        {
            public string Type { get; set; }
        }

        ModuloRiskManagerConfig Config;
        HttpClient Client;

        public CallModuloRiskManagerAnonymously(Func<HttpClient> clientFactory, ModuloRiskManagerConfig config)
        {
            Client = clientFactory();
            Client.BaseAddress = config.BaseUri;
            Config = config;
        }

        public void Run(Parameters parameters)
        {
            AsyncHelper.RunSync(async () =>
            {
                await Client.GetAndInstallAnonymousToken(Config);
                //do something with the Modulo Risk Manager
            });
        }
    }

    public class ModuloRiskManagerConfig
    {
        public Uri BaseUri { get; set; }
        public string Id { get; set; }
        public string Key { get; set; }
    }

    public class TokenResponseDto
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }

    
}
