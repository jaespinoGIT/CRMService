﻿using Autofac;
using CRMService.Core.Exceptions.Services;
using CRMService.Core.Services;
using CRMService.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMService.Core
{
    public class CoreModule : Module
    {
        //Autofac configuration
        protected override void Load(ContainerBuilder bldr)
        {        

            bldr.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();            

            bldr.RegisterType<CustomExceptionService>().As<ICustomExceptionService>().InstancePerLifetimeScope();

        }
    }
}
