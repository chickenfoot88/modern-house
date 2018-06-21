using Owin;
using SimpleInjector;

namespace Tbo.WebHost
{
    public partial class Startup
    {
        // Дополнительные сведения о настройке проверки подлинности см. по адресу: http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app, Container container)
        {
            Core.Identity.OAuth.OWIN.Startup.AuthCofig.ConfigureAuth(app, container);
        }
    }
}