using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Core.DataAccess.Params;
using Domain.Core.Exceptions;
using Domain.Dictionary.ContainerTypes.Entities;
using Domain.Dictionary.ContainerTypes.Interfaces;
using Domain.Dictionary.ContainerTypes.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace Domain.Dictionary.ContainerTypes.Services
{
    public class ContainerTypeService: IContainerTypeService
    {
        private readonly IDataStore dataStore;

        public ContainerTypeService(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public List<ContainerTypeGetModel> GetAllContainerTypeModels(StoreLoadParams storeLoadParams)
        {
            return dataStore.GetAll<ContainerType>()
                .Select(ContainerTypeGetModel.ProjectionExpression)
                .Paging(storeLoadParams)
                .ToList();
        }

        public ContainerTypeGetModel GetContainerTypeModel(long id)
        {
            var containerType = dataStore.Get<ContainerType>(id);

            return containerType != null
                ? new ContainerTypeGetModel(containerType)
                : null;
        }

        public void Create(ContainerTypeSaveModel containerTypeModel)
        {
            var containerType = new ContainerType();

            containerTypeModel.ApplyToEntity(containerType);

            dataStore.Save(containerType);
        }

        public async Task CreateAsync(ContainerTypeSaveModel containerTypeModel)
        {
            var containerType = new ContainerType();

            containerTypeModel.ApplyToEntity(containerType);

            await dataStore.SaveAsync(containerType);
        }

        public void Update(long id, ContainerTypeSaveModel containerTypeModel)
        {
            var containerType = dataStore.Get<ContainerType>(id);

            if (containerType == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(ContainerType).Name} c идентификатором {id} не существует");
            }

            containerTypeModel.ApplyToEntity(containerType);

            dataStore.SaveChanges();
        }

        public async Task UpdateAsync(long id, ContainerTypeSaveModel containerTypeModel)
        {
            var containerType = dataStore.Get<ContainerType>(id);

            if (containerType == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(ContainerType).Name} c идентификатором {id} не существует");
            }

            containerTypeModel.ApplyToEntity(containerType);

            await dataStore.SaveChangesAsync();
        }

        public void Delete(long id)
        {
            var containerType = dataStore.Get<ContainerType>(id);

            if (containerType == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(ContainerType).Name} c идентификатором {id} не существует");
            }

            dataStore.Delete(containerType);
        }

        public async Task DeleteAsync(long id)
        {
            var containerType = dataStore.Get<ContainerType>(id);

            if (containerType == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(ContainerType).Name} c идентификатором {id} не существует");
            }

            await dataStore.DeleteAsync(containerType);
        }

        public static void CreateTestData(IDataStore dataStore)
        {
            if (!Settings.TestEnvironment || dataStore.GetAll<ContainerType>().Any())
            {
                return;
            }

            var containerTypes = new[]
            {
                new ContainerType { Name="БН-8", Capacity=8.0m },
                new ContainerType { Name="БН-10", Capacity=10.0m },
                new ContainerType { Name="БН-13", Capacity=13.0m },
                new ContainerType { Name="БН-15", Capacity=15.0m },
                new ContainerType { Name="БН-20", Capacity=20.0m }
            };

            foreach (var containerType in containerTypes)
            {
                dataStore.Save(containerType);
            }
        }
    }
}
