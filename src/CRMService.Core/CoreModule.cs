using Autofac;
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
        protected override void Load(ContainerBuilder bldr)
        {        

            bldr.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();   

            bldr.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

        }
    }
}
