using System.Collections.Generic;
using Domain.Registries.Monitoring.Models;

namespace Domain.Registries.Requests.Interfaces
{
    public interface IMonitoringService
    {
        List<CarMonitoringGetModel> GetAllCarsPosition();
        List<ContainerMonitoringGetModel> GetAllContainersPosition();
    }
}
