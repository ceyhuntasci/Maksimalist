using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Maksimalist.Startup))]
namespace Maksimalist
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
