using System.Collections.Generic;
using System.Linq;
using Core;
using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Core.DataAccess.Params;
using Domain.Core.Exceptions;
using Domain.Dictionary.Containers.Entities;
using Domain.Dictionary.Containers.Enums;
using Domain.Dictionary.Containers.Interfaces;
using Domain.Dictionary.Containers.Models;
using Domain.Core.Positions.Services;
using System.Threading.Tasks;
using Domain.Core.Positions.Models;
using Domain.Core.Positions.Interfaces;

namespace Domain.Dictionary.Containers.Services
{
    public class ContainerService : IContainerService
    {
        private readonly IDataStore dataStore;
        private readonly IPositionService positionService;

        public ContainerService(IDataStore dataStore, PositionService positionService)
        {
            this.dataStore = dataStore;
            this.positionService = positionService;
        }

        public List<ContainerGetModel> GetAllContainerModels(StoreLoadParams storeLoadParams)
        {
            return dataStore.GetAll<Container>()
                .Select(ContainerGetModel.ProjectionExpression)
                .Paging(storeLoadParams)
                .ToList();
        }

        public List<ContainerGetModel> GetAllFreeContainerModels(StoreLoadParams storeLoadParams)
        {
            return dataStore.GetAll<Container>()
                .Where(x=>x.Status==ContainerStatus.Free)
                .Select(ContainerGetModel.ProjectionExpression)
                .Paging(storeLoadParams)
                .ToList();
        }

        public ContainerGetModel GetContainerModel(long id)
        {
            var container = dataStore.Get<Container>(id);

            return container != null
                ? new ContainerGetModel(container)
                : null;
        }

        public void Create(ContainerSaveModel containerModel)
        {
            containerModel.Number = GetNextNumber();
            var container = new Container();

            containerModel.ApplyToEntity(container, dataStore, positionService);

            dataStore.Save(container);
        }

        private long GetNextNumber()
        {
            return dataStore.GetAll<Container>()
                       .Select(x => (long?)(x.Number + 1))
                       .OrderByDescending(x => x)
                       .Max() ?? 1;
        }

        public async Task CreateAsync(ContainerSaveModel containerModel)
        {
            containerModel.Number = GetNextNumber();
            var container = new Container();

            containerModel.ApplyToEntity(container, dataStore, positionService);

            await dataStore.SaveAsync(container);
        }

        public void Update(long id, ContainerSaveModel containerModel)
        {
            var container = dataStore.Get<Container>(id);

            if (container == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Container).Name} c идентификатором {id} не существует");
            }

            containerModel.ApplyToEntity(container, dataStore, positionService);

            dataStore.SaveChanges();
        }

        public async Task UpdateAsync(long id, ContainerSaveModel containerModel)
        {
            var container = dataStore.Get<Container>(id);

            if (container == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Container).Name} c идентификатором {id} не существует");
            }

            containerModel.ApplyToEntity(container, dataStore, positionService);

            await dataStore.SaveChangesAsync();
        }

        public void UpdatePosition(long id, PositionModel positionModel)
        {
            var container = dataStore.Get<Container>(id);

            if (container == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Container).Name} c идентификатором {id} не существует");
            }

            if (container.PositionId.HasValue)
            {
                positionService.Update(container.PositionId.Value, positionModel.Latitude, positionModel.Longitude);
            }
            else
            {
                container.Position = positionService.Create(positionModel.Latitude, positionModel.Longitude);
                dataStore.SaveChanges();
            }
        }

        public async Task UpdatePositionAsync(long id, PositionModel positionModel)
        {
            var container = dataStore.Get<Container>(id);

            if (container == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Container).Name} c идентификатором {id} не существует");
            }

            if (container.PositionId.HasValue)
            {
                await positionService.UpdateAsync(container.PositionId.Value, positionModel.Latitude, positionModel.Longitude);
            }
            else
            {
                container.Position = positionService.Create(positionModel.Latitude, positionModel.Longitude);
                await dataStore.SaveChangesAsync();
            }
        }

        public void Delete(long id)
        {
            var container = dataStore.Get<Container>(id);

            if (container == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Container).Name} c идентификатором {id} не существует");
            }
            if (container.PositionId.HasValue)
            {
                positionService.Delete(container.PositionId.Value);
            }

            dataStore.Delete(container);
        }

        public async Task DeleteAsync(long id)
        {
            var container = dataStore.Get<Container>(id);

            if (container == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Container).Name} c идентификатором {id} не существует");
            }
            if (container.PositionId.HasValue)
            {
                await positionService.DeleteAsync(container.PositionId.Value);
            }

            await dataStore.DeleteAsync(container);
        }

        public static void CreateTestData(IDataStore dataStore)
        {
            if (!Settings.TestEnvironment || dataStore.GetAll<Container>().Any())
            {
                return;
            }

            var containerTypes = dataStore.GetAll<ContainerTypes.Entities.ContainerType>()
                .ToList();

            var counter = 1;
            foreach (var item in Enumerable.Range(1, 44))
            {
                var container = new Container { Number = counter++, Status = ContainerStatus.Free, ContainerType = containerTypes.FirstOrDefault(x => x.Capacity == 8.0m) };
                dataStore.Save(container);
            }
            foreach (var item in Enumerable.Range(1, 4))
            {
                var container = new Container { Number = counter++, Status = ContainerStatus.Free, ContainerType = containerTypes.FirstOrDefault(x => x.Capacity == 13.0m) };
                dataStore.Save(container);
            }
            foreach (var item in Enumerable.Range(1, 4))
            {
                var container = new Container { Number = counter++, Status = ContainerStatus.Free, ContainerType = containerTypes.FirstOrDefault(x => x.Capacity == 15.0m) };
                dataStore.Save(container);
            }
            foreach (var item in Enumerable.Range(1, 10))
            {
                var container = new Container { Number = counter++, Status = ContainerStatus.Free, ContainerType = containerTypes.FirstOrDefault(x => x.Capacity == 20.0m) };
                dataStore.Save(container);
            }
        }
    }
}
