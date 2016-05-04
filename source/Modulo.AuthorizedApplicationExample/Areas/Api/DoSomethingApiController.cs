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
    }
}
