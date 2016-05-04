using System;
using System.Net.Http;
using System.Threading;

namespace Modulo.AuthorizedApplicationExample.Services
{
    public class DoSomething
    {
        public class Parameters
        {
            public string Type { get; set; }
        }

        HttpClient Client;
        int SomeDependency;

        public DoSomething(HttpClient client, int someDependency)
        {
            Client = client;
            SomeDependency = someDependency;
        }

        public void Run(Parameters parameters)
        {
            AsyncHelper.RunSync(async () =>
            {
                Console.WriteLine(SomeDependency);

                if (parameters.Type.ToLower() == "fast")
                {
                }
                else if (parameters.Type.ToLower() == "slow")
                {
                    Thread.Sleep(10000);
                }
                else if (parameters.Type.ToLower() == "error")
                {
                    var response = await Client.GetAsync("http://www.someimaginaryserver.com");

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception();
                    }
                }
            });
        }
    }
}
