using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EniEvents.Startup))]
namespace EniEvents
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
