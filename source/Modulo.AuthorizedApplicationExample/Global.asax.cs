using Microsoft.Practices.Unity;
using System;
using System.Web.Http;
using Unity.WebApi;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.Unity;
using Serilog;
using Microsoft.Owin;
using Owin;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Net;
using System.Configuration;
using System.Collections.Generic;
using WebApiContrib.Formatting.Razor;
using WebApiContrib.Formatting.Html;
using System.Net.Http.Formatting;

[assembly: OwinStartup(typeof(Modulo.AuthorizedApplicationExample.Startup))]

namespace Modulo.AuthorizedApplicationExample
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }
    }

    public delegate string ToPhysicalPath(string virtualPath);

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var unity = new UnityContainer();
            unity.RegisterInstance<int>(42);
            unity.RegisterInstance<HttpClient>(new HttpClient());
            unity.RegisterInstance<ToPhysicalPath>(new ToPhysicalPath(x =>
                {
                    return System.Web.Hosting.HostingEnvironment.MapPath(x);
                }));

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();

            var webApiConfiguration = System.Web.Http.GlobalConfiguration.Configuration;
            webApiConfiguration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
            webApiConfiguration.Services.Add(typeof(IExceptionLogger), new SerilogExceptionLogger());
            webApiConfiguration.DependencyResolver = new UnityDependencyResolver(unity);
            webApiConfiguration.MapHttpAttributeRoutes();
            webApiConfiguration.Formatters.Add(new FormUrlEncodedMediaTypeFormatter());
            webApiConfiguration.Formatters.Add(new RazorViewFormatter());
            webApiConfiguration.EnsureInitialized();

            var hangfireConfiguration = Hangfire.GlobalConfiguration.Configuration;
            hangfireConfiguration.UseMemoryStorage(new MemoryStorageOptions()
            {
            });
            hangfireConfiguration.UseActivator(new UnityJobActivator(unity));
            hangfireConfiguration.UseSerilogLogProvider();
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 3 });

            app.UseHangfireDashboard("/admin/hangfire", new DashboardOptions()
            {
                AuthorizationFilters = new Hangfire.Dashboard.IAuthorizationFilter[] { new LocalhostOnlyAuthorizeAttribute() }
            });
            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                WorkerCount = 1
            });
        }
    }

    public class LocalhostOnlyAuthorizeAttribute : FilterAttribute, Hangfire.Dashboard.IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            return bool.Parse(owinEnvironment["server.IsLocal"].ToString());
        }
    }

    public class ConfigKeyAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        HttpResponseMessage GetUnauthorized()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            return response;
        }


        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var header = actionContext.Request.Headers.Authorization;

            if (header == null)
            {
                return Task.FromResult(GetUnauthorized());
            }

            if (header.Scheme != "Bearer")
            {
                return Task.FromResult(GetUnauthorized());
            }

            var key = ConfigurationManager.AppSettings["ApiKey"];

            if (header.Parameter != key)
            {
                return Task.FromResult(GetUnauthorized());
            }

            return continuation();
        }
    }

    class SerilogExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            Serilog.Log.Logger.Error(context.Exception, "WebApi Error");
        }
    }
}