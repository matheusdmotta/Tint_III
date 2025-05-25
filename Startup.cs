using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fim.Startup))]
namespace Fim
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
