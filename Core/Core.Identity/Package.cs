using Core.Identity.Entities;
using Core.Identity.Services;
using Microsoft.AspNet.Identity;
using SimpleInjector;

namespace Core.Identity
{
    public class Package : SimpleInjector.Packaging.IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<UserStore>(Lifestyle.Transient);
            container.Register<UserManager<ApplicationUser, long>, ApplicationUserManager>(Lifestyle.Transient);
            container.Register<ApplicationUserManager>(Lifestyle.Transient);
            container.Register<PermissionService>(Lifestyle.Transient);
        }

        public static void Bootstrap()
        {
        }
    }
}
