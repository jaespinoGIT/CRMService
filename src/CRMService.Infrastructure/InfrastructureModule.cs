using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CRMService.Core.Repositories;

using CRMService.Infrastructure.Data.EntityFramework;
using CRMService.Infrastructure.Data.EntityFramework.Repositories;


namespace CRMService.Infrastructure
{
    public class InfrastructureModule : Module
    {
        //Autofac configuration
        protected override void Load(ContainerBuilder bldr)
        {
            bldr.RegisterType<DataContext>()
             .InstancePerLifetimeScope();

            bldr.RegisterType<CustomerRepository>()
               .As<ICustomerRepository>()
               .InstancePerLifetimeScope();      
        }
    }
}
