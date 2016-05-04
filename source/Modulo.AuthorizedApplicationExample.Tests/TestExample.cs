using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modulo.AuthorizedApplicationExample.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Modulo.AuthorizedApplicationExample.Tests
{
    [TestClass]
    public class TestExample
    {
        /// <summary>
        /// Mesmo o método chamando um serviço que não existe é possível
        /// simular uma chamada com sucesso.
        /// </summary>
        [TestMethod]
        public void SimulateOK()
        {
            var client = ReplayHandler.GetClient(SimulateCallWorked());

            var sut = new DoSomething(client, 42);
            sut.Run(new DoSomething.Parameters()
            {
                Type = "error"
            });
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SimulateNotFound()
        {
            var client = ReplayHandler.GetClient(SimulateCallNotFound());

            var sut = new DoSomething(client, 42);
            sut.Run(new DoSomething.Parameters()
            {
                Type = "error"
            });
        }

        IEnumerable<HttpResponseMessage> SimulateCallWorked()
        {
            yield return new HttpResponseMessage(HttpStatusCode.OK);
        }

        IEnumerable<HttpResponseMessage> SimulateCallNotFound()
        {
            yield return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }

    public class ReplayHandler : HttpMessageHandler
    {
        public static HttpClient GetClient(IEnumerable<HttpResponseMessage> responses)
        {
            return new HttpClient(new ReplayHandler(responses));
        }

        IEnumerator<HttpResponseMessage> Enumerator;

        public ReplayHandler(IEnumerable<HttpResponseMessage> responses)
        {
            Enumerator = responses.GetEnumerator();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Enumerator.MoveNext();
            return Task.FromResult(Enumerator.Current);
        }
    }
}
