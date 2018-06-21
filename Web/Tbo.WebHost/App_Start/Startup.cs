using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using Tbo.WebHost;
using Tbo.WebHost.Filters;

[assembly: OwinStartup(typeof(Startup))]
namespace Tbo.WebHost
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var apiConfig = ConfigureWebApi();

            // DI
            var container = SimpleInjectorConfig.GetConfiguredContainer();
            container.RegisterMvcControllers(this.GetType().Assembly);
            container.RegisterWebApiControllers(apiConfig);
            container.Verify(VerificationOption.VerifyOnly);

            //Web Api
            apiConfig.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            ConfigureMvc(container);

            ConfigureAuth(app, container);

            JsonConverterConfig.Configure();

            app.UseWebApi(apiConfig);
        }

        private void ConfigureMvc(SimpleInjector.Container container)
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            System.Web.Mvc.DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();

            SwaggerConfig.Register(config);

            config.Filters.Add(new WebApiExceptionFilter());

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            return config;
        }
    }
}
