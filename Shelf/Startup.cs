using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shelf.Startup))]
namespace Shelf
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
