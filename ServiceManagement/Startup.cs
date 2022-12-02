using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ServiceManagement.Startup))]
namespace ServiceManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
