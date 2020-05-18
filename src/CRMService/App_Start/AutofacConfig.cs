using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using CRMService.Core;
using CRMService.Infrastructure;
using CRMService.Models;
using System.Reflection;
using System.Web.Http;

namespace CRMService
{
    /// <summary>
    /// Autofac dependency inyection configuration
    /// </summary>
    public class AutofacConfig
    {
        public static void Register()
        {
            var bldr = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            bldr.RegisterApiControllers(Assembly.GetExecutingAssembly());
            RegisterServices(bldr);
            bldr.RegisterWebApiFilterProvider(config);
            bldr.RegisterWebApiModelBinderProvider();
            var container = bldr.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterServices(ContainerBuilder bldr)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntitiesMappingProfile());
            });                       

            bldr.RegisterInstance(config.CreateMapper())
            .As<IMapper>()
            .SingleInstance();
            //Register core and infracstructure dependencies
            bldr.RegisterModule(new CoreModule());
            bldr.RegisterModule(new InfrastructureModule());      

        }
    }
}
