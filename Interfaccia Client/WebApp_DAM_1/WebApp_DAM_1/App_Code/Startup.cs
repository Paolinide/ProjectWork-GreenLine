using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApp_DAM_1.Startup))]
namespace WebApp_DAM_1
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
