using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoR.Startup))]
namespace PhotoR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
