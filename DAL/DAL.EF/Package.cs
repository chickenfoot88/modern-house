using Core.DataAccess.Interfaces;
using SimpleInjector;

namespace DAL.EF
{
    public class Package : SimpleInjector.Packaging.IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<ApplicationDbContext>(Lifestyle.Transient);
            container.Register<IDataStore, DAL.EF.Implementations.DataStore>(Lifestyle.Transient);
        }

        public static void Bootstrap()
        {
            
        }
    }
}
