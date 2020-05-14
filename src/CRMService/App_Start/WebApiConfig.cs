using CRMService.Helpers;
using CRMService.Helpers.Filters;
using CRMService.Helpers.Serialization;
using Microsoft.Web.Http.Versioning;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using WebApiThrottle;

namespace CRMService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services           
            AutofacConfig.Register();
            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new ItemNotFoundExceptionFilterAttribute());

            //Anti xss config
            config.Formatters.JsonFormatter.SerializerSettings.Converters = new List<JsonConverter>
            {
                new AntiXssConverter()
            };
            config.Formatters.JsonFormatter.SerializerSettings.StringEscapeHandling =
    StringEscapeHandling.EscapeHtml;

            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            //Avoid child auto reference
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
                = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //Api versioning
            config.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new Microsoft.Web.Http.ApiVersion(1, 1);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;
                cfg.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("X-Version"));
            });
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //WebApiThrottle
            config.MessageHandlers.Add(new ThrottlingHandler()
            {
                Policy = new ThrottlePolicy(perSecond: 5, perMinute: 30)
                {
                    IpThrottling = true,
                    ClientThrottling = true,
                    EndpointThrottling = true
                },
                Repository = new CacheRepository()
            });

        }
    }
}
