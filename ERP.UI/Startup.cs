using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ERP.Startup))]
namespace ERP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
