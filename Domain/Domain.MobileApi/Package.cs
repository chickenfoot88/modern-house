using Domain.MobileApi.Interfaces;
using Domain.MobileApi.Services;
using SimpleInjector;

namespace Domain.MobileApi
{
    public class Package : SimpleInjector.Packaging.IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IDriverUserService, DriverUserService>(Lifestyle.Transient);
            container.Register<IDriverRequestsService, DriverRequestsService>(Lifestyle.Transient);
        }

        public static void Bootstrap()
        {

        }
    }
}
