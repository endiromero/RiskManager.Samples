using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Modulo.AuthorizedApplicationExample.Services
{
    public class CallModuloRiskManagerLoggedIn
    {
        public class Parameters
        {
            public string Type { get; set; }
        }

        ModuloRiskManagerConfig Config;
        ModuloRiskManagerUserContext UserContext;
        HttpClient Client;

        public CallModuloRiskManagerLoggedIn(Func<HttpClient> clientFactory, ModuloRiskManagerConfig config, ModuloRiskManagerUserContext userContext)
        {
            Client = clientFactory();
            Client.BaseAddress = config.BaseUri;
            Config = config;
        }

        public void Run(Parameters parameters)
        {
            AsyncHelper.RunSync(async () =>
            {
                Client.GetAndInstallUserContext(Config, UserContext);
                
                //do something with the Modulo Risk Manager
                await Task.Yield();
            });
        }
    }    

    
}
