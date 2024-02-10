using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SiteDeVente.Startup))]
namespace SiteDeVente
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

}
