using System.Collections.Generic;
using Domain.Registries.Requests.Interfaces;
using Core.DataAccess.Interfaces;
using System.Linq;
using Domain.Dictionary.Cars.Entities;
using Domain.Dictionary.Containers.Entities;
using Domain.Registries.Monitoring.Models;

namespace Domain.Registries.Requests.Services
{
    public class MonitoringService : IMonitoringService
    {
        private readonly IDataStore dataStore;

        public MonitoringService(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public List<CarMonitoringGetModel> GetAllCarsPosition()
        {
            return dataStore.GetAll<Car>()
                .Where(x => x.Position != null)
                .Select(CarMonitoringGetModel.ProjectionExpression)
                .ToList();
        }

        public List<ContainerMonitoringGetModel> GetAllContainersPosition()
        {
            return dataStore.GetAll<Container>()
                .Where(x=>x.Position!=null)
                .Select(ContainerMonitoringGetModel.ProjectionExpression)
                .ToList();
        }
    }
}
