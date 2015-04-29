using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ENetCareMVC.Web.Startup))]
namespace ENetCareMVC.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
