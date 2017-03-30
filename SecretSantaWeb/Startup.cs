using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SecretSantaWeb.Startup))]
namespace SecretSantaWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
