using Domain.Dictionary.Cars.Interfaces;
using Domain.Dictionary.Cars.Services;
using Domain.Dictionary.Containers.Interfaces;
using Domain.Dictionary.Containers.Services;
using Domain.Dictionary.ContainerTypes.Interfaces;
using Domain.Dictionary.ContainerTypes.Services;
using Domain.Dictionary.Customers.Interfaces;
using Domain.Dictionary.Customers.Services;
using Domain.Dictionary.Drivers.Interfaces;
using Domain.Dictionary.Drivers.Services;
using Domain.Dictionary.Polygons.Interfaces;
using Domain.Dictionary.Polygons.Services;
using SimpleInjector;

namespace Domain.Dictionary
{
    public class Package : SimpleInjector.Packaging.IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<ICarService, CarService>(Lifestyle.Transient);
            container.Register<IContainerService, ContainerService>(Lifestyle.Transient);
            container.Register<IContainerTypeService, ContainerTypeService>(Lifestyle.Transient);
            container.Register<ICustomerService, CustomerService>(Lifestyle.Transient);
            container.Register<IDriverService, DriverService>(Lifestyle.Transient);
            container.Register<IPolygonService, PolygonService>(Lifestyle.Transient);
        }

        public static void Bootstrap()
        {

        }
    }
}
