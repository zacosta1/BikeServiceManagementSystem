using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BSMSWebsite.Startup))]
namespace BSMSWebsite
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
