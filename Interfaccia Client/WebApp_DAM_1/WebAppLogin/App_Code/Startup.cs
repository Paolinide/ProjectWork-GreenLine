using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppLogin.Startup))]
namespace WebAppLogin
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
