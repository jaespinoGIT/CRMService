using CRMService.Helpers.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace CRMService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Filters.Add(
    new NullModelStateActionFilter
    {
        ReturnsBadRequest = true
    }
);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
