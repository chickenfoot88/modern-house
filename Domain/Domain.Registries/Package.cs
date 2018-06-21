using Domain.Registries.Requests.Interfaces;
using Domain.Registries.Requests.Services;
using Domain.Registries.Schedules.Interfaces;
using Domain.Registries.Schedules.Services;
using Domain.Registries.Waybills.Interfaces;
using Domain.Registries.Waybills.Services;
using SimpleInjector;

namespace Domain.Registries
{
    public class Package : SimpleInjector.Packaging.IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IRequestService, RequestService>(Lifestyle.Transient);
            container.Register<IWaybillService, WaybillService>(Lifestyle.Transient);
            container.Register<ISchedulesService, SchedulesService>(Lifestyle.Transient);
            container.Register<IMonitoringService, MonitoringService>(Lifestyle.Transient);
        }
        public static void Bootstrap()
        {

        }
    }
}
