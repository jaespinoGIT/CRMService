using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CRMService.Startup))]

namespace CRMService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);             
        }   
    }
}
