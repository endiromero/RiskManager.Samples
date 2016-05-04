using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApiContrib.Formatting.Html;

namespace Modulo.AuthorizedApplicationExample.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet, Route("~/")]
        public IHttpActionResult Index()
        {
            return new ViewResult(Request, "Index", null);
        }
    }
}