using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Payroll.Startup))]
namespace Payroll
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
