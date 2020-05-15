using System;
using System.Net;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CRMService.Infrastructure.Data.EntityFramework;


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CRMServiceApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("CRMService.Infrastructure")));
            services.AddAutoMapper();


            // Now register our services with Autofac container.
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new InfrastructureModule());

            // Presenters
            //builder.RegisterType<RegisterUserPresenter>().SingleInstance();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Presenter")).SingleInstance();

            builder.Populate(services);
            var container = builder.Build();
            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
