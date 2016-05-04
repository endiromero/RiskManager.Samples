using Hangfire;
using System.Web.Http;
using System;
using Modulo.AuthorizedApplicationExample.Services;
using Microsoft.Practices.Unity;

namespace Modulo.AuthorizedApplicationExample.Areas.Api
{
    [ConfigKeyAuthorizeAttribute]
    public class DoSomethingApiController : ApiController
    {
        Func<IUnityContainer> GetContainer;

        public DoSomethingApiController(Func<IUnityContainer> getContainer)
        {
            GetContainer = getContainer;
        }

        [HttpPost, Route("~/api/dosomething")]
        public IHttpActionResult DoSomethingAction(DoSomething.Parameters parameters)
        {
            BackgroundJob.Enqueue(() => DoSomething(parameters));
            return Ok();
        }

        [HttpPost, Route("~/api/simulateerror")]
        public IHttpActionResult SimulateError(DoSomething.Parameters parameters)
        {
            throw new Exception();
        }

        [DisableConcurrentExecution(timeoutInSeconds: 600)]
        public void DoSomething(DoSomething.Parameters parameters)
        {
            var service = GetContainer().Resolve<DoSomething>();
            service.Run(parameters);
        }

        [HttpPost, Route("~/api/callmoduloriskmanageranonymously")]
        public IHttpActionResult CallModuloRiskManagerAnonymously(DoSomething.Parameters parameters)
        {
            BackgroundJob.Enqueue(() => CallModuloRiskManagerAnonymously(parameters));
            return Ok();
        }

        [DisableConcurrentExecution(timeoutInSeconds: 600)]
        public void CallModuloRiskManagerAnonymously(CallModuloRiskManagerAnonymously.Parameters parameters)
        {
            var service = GetContainer().Resolve<CallModuloRiskManagerAnonymously>();
            service.Run(parameters);
        }

        [HttpPost, Route("~/api/callmoduloriskmanagerloggedin")]
        public IHttpActionResult CallModuloRiskManagerLoggedIn(DoSomething.Parameters parameters)
        {
            BackgroundJob.Enqueue(() => CallModuloRiskManagerLoggedIn(parameters));
            return Ok();
        }

        [DisableConcurrentExecution(timeoutInSeconds: 600)]
        public void CallModuloRiskManagerLoggedIn(CallModuloRiskManagerLoggedIn.Parameters parameters)
        {
            var service = GetContainer().Resolve<CallModuloRiskManagerLoggedIn>();
            service.Run(parameters);
        }
    }
}
