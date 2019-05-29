using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppLoginPork.Startup))]
namespace WebAppLoginPork
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
