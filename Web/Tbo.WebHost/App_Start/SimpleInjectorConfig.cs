using System;
using SimpleInjector;
using SimpleInjector.Integration.Web;

namespace Tbo.WebHost
{
    public static class SimpleInjectorConfig
    {
        public static SimpleInjector.Container GetConfiguredContainer()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            BootstrapAssemblies();
            container.RegisterPackages(AppDomain.CurrentDomain.GetAssemblies());
            return container;
        }

        private static void BootstrapAssemblies()
        {
            Core.Identity.Package.Bootstrap();
            DAL.EF.Package.Bootstrap();
            FileManager.FileSystem.Package.Bootstrap();

            Domain.Core.Package.Bootstrap();
            Domain.Dictionary.Package.Bootstrap();
            Domain.Registries.Package.Bootstrap();
            Domain.MobileApi.Package.Bootstrap();
        }
    }
}