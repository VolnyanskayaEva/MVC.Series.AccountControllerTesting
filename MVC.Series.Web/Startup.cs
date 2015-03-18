using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC.Series.Web.Startup))]
namespace MVC.Series.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
