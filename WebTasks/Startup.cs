using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebTasks.Startup))]
namespace WebTasks
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
