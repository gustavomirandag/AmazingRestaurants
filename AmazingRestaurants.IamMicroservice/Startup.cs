using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AmazingRestaurants.IamMicroservice.Startup))]
namespace AmazingRestaurants.IamMicroservice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
