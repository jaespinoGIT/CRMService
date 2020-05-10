﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using CRMService.Core;
using CRMService.Core.Repositories;
using CRMService.Core.Services;
using CRMService.Core.Services.Interfaces;
using CRMService.Infrastructure;
using CRMService.Infrastructure.Data.EntityFramework;
using CRMService.Infrastructure.Data.EntityFramework.Repositories;
using CRMService.Models;

namespace CRMService
{
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
                cfg.AddProfile(new DataMappingProfile());
            });                       

            bldr.RegisterInstance(config.CreateMapper())
            .As<IMapper>()
            .SingleInstance();

            bldr.RegisterModule(new CoreModule());
            bldr.RegisterModule(new InfrastructureModule());      

        }
    }
}
