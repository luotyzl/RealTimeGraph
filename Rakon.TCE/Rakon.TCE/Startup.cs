using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rakon.TCE.Startup))]
namespace Rakon.TCE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
