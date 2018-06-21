using Domain.Core.Positions.Interfaces;
using Domain.Core.Positions.Services;
using SimpleInjector;

namespace Domain.Core
{
    public class Package : SimpleInjector.Packaging.IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IPositionService, PositionService>(Lifestyle.Transient);
        }

        public static void Bootstrap()
        {

        }
    }
}
