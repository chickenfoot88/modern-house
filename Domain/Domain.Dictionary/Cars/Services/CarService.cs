using System.Linq;
using Core;
using Core.DataAccess.Interfaces;
using Domain.Core.Exceptions;
using Domain.Dictionary.Cars.Entities;
using Domain.Dictionary.Cars.Interfaces;
using Domain.Dictionary.Cars.Models;
using Domain.Dictionary.Containers.Entities;
using Domain.Dictionary.Containers.Services;
using System.Collections.Generic;
using Core.DataAccess.Extensions;
using Core.DataAccess.Params;
using Domain.Core.Positions.Models;
using Domain.Core.Positions.Services;
using System.Threading.Tasks;
using Domain.Core.Positions.Interfaces;
using Domain.Dictionary.ContainerTypes.Entities;
using Domain.Dictionary.ContainerTypes.Models;
using Domain.Dictionary.ContainerTypes.Services;

namespace Domain.Dictionary.Cars.Services
{
    public class CarService : ICarService
    {
        private readonly IDataStore dataStore;
        private readonly IPositionService positionService;

        public CarService(IDataStore dataStore, PositionService positionService)
        {
            this.dataStore = dataStore;
            this.positionService = positionService;
        }

        public List<CarGetModel> GetAllCarModels(StoreLoadParams storeLoadParams)
        {
            return dataStore.GetAll<Car>()
                .Select(CarGetModel.ProjectionExpression)
                .Paging(storeLoadParams)
                .ToList();
        }

        public CarGetModel GetCarModel(long id)
        {
            var car = dataStore.Get<Car>(id);

            return car != null
                ? new CarGetModel(car)
                : null;
        }

        public List<CarContainerTypeGetModel> GetAllCarContainerTypeModels(long carId, StoreLoadParams storeLoadParams)
        {
            var result = dataStore.GetAll<CarContainerType>()
                .Where(x => x.CarId == carId)
                .Select(CarContainerTypeGetModel.ProjectionExpression)
                .Paging(storeLoadParams)
                .ToList();

            return result;
        }

        public List<ContainerTypeGetModel> GetAvailableContainerTypeModels(long carId, StoreLoadParams storeLoadParams)
        {
            var existing = dataStore.GetAll<CarContainerType>()
                .Where(x => x.CarId == carId)
                .Select(x => x.ContainerTypeId)
                .ToList();

            var available = dataStore.GetAll<ContainerType>()
                .Where(x => !existing.Contains(x.Id))
                .Select(ContainerTypeGetModel.ProjectionExpression)
                .Paging(storeLoadParams)
                .ToList();

            return available;
        }

        public void Create(CarSaveModel carModel)
        {
            var car = new Car();

            carModel.ApplyToEntity(car, dataStore, positionService);

            dataStore.Save(car);
        }

        public async Task CreateAsync(CarSaveModel carModel)
        {
            var car = new Car();

            carModel.ApplyToEntity(car, dataStore, positionService);

            await dataStore.SaveAsync(car);
        }

        public void Update(long id, CarSaveModel carModel)
        {
            var car = dataStore.Get<Car>(id);

            if (car == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Car).Name} c идентификатором {id} не существует");
            }

            carModel.ApplyToEntity(car, dataStore, positionService);

            dataStore.SaveChanges();
        }

        public async Task UpdateAsync(long id, CarSaveModel carModel)
        {
            var car = dataStore.Get<Car>(id);

            if (car == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Car).Name} c идентификатором {id} не существует");
            }

            carModel.ApplyToEntity(car, dataStore, positionService);

            await dataStore.SaveChangesAsync();
        }

        public void UpdatePosition(long id, PositionModel positionModel)
        {
            var car = dataStore.Get<Car>(id);

            if (car == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Car).Name} c идентификатором {id} не существует");
            }

            if (car.PositionId.HasValue)
            {
                positionService.Update(car.PositionId.Value, positionModel.Latitude, positionModel.Longitude);
            }
            else
            {
                car.Position = positionService.Create(positionModel.Latitude, positionModel.Longitude);
                dataStore.SaveChanges();
            }
        }

        public async Task UpdatePositionAsync(long id, PositionModel positionModel)
        {
            var car = dataStore.Get<Car>(id);

            if (car == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Car).Name} c идентификатором {id} не существует");
            }

            if (car.PositionId.HasValue)
            {
                await positionService.UpdateAsync(car.PositionId.Value, positionModel.Latitude, positionModel.Longitude);
            }
            else
            {
                car.Position = positionService.Create(positionModel.Latitude, positionModel.Longitude);
                await dataStore.SaveChangesAsync();
            }
        }

        public void Delete(long id)
        {
            var car = dataStore.Get<Car>(id);

            if (car == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Car).Name} c идентификатором {id} не существует");
            }
            if (car.PositionId != null)
            {
                positionService.Delete(car.PositionId.Value);
            }

            dataStore.Delete(car);
        }

        public async Task DeleteAsync(long id)
        {
            var car = dataStore.Get<Car>(id);

            if (car == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Car).Name} c идентификатором {id} не существует");
            }
            if (car.PositionId != null)
            {
                await positionService.DeleteAsync(car.PositionId.Value);
            }

            await dataStore.DeleteAsync(car);
        }

        public void AddCarContainerType(long id, long containerTypeId)
        {
                if (dataStore.GetAll<CarContainerType>().Any(x => x.CarId == id && x.ContainerTypeId == containerTypeId))
                {
                    throw new KeyExistsException("Для автомобиля уже добавлен указанный тип контейнера");
                }

                var carContainerType = new CarContainerType
                {
                    CarId = id,
                    ContainerTypeId = containerTypeId
                };

                dataStore.Save(carContainerType);
        }

        public async Task AddCarContainerTypeAsync(long id, long containerTypeId)
        {
            if (dataStore.GetAll<CarContainerType>().Any(x => x.CarId == id && x.ContainerTypeId == containerTypeId))
            {
                throw new KeyExistsException("Для автомобиля уже добавлен указанный тип контейнера");
            }

            var carContainerType = new CarContainerType
            {
                CarId = id,
                ContainerTypeId = containerTypeId
            };

            await dataStore.SaveAsync(carContainerType);
        }

        public void DeleteCarContainerType(long id)
        {
            var carContainerType = dataStore.Get<CarContainerType>(id);

            if (carContainerType == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(CarContainerType).Name} c идентификатором {id} не существует");
            }

            dataStore.Delete(carContainerType);
        }

        public async Task DeleteCarContainerTypeAsync(long id)
        {
            var carContainerType = dataStore.Get<CarContainerType>(id);

            if (carContainerType == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(CarContainerType).Name} c идентификатором {id} не существует");
            }

            await dataStore.DeleteAsync(carContainerType);
        }

        public static void CreateTestData(IDataStore dataStore)
        {
            if (!Settings.TestEnvironment || dataStore.GetAll<Car>().Any())
            {
                return;
            }

            var cars = new[]
            {
                new Car {Number = "Р 261 ТК", Mark = "МАЗ", Serviceability = Enums.CarServiceability.Serviceable},
                new Car {Number = "С 013 НЕ", Mark = "МАЗ", Serviceability = Enums.CarServiceability.Serviceable},
                new Car {Number = "Х 773 КМ", Mark = "МАЗ", Serviceability = Enums.CarServiceability.Serviceable},
                new Car {Number = "А 509 ВН", Mark = "МАЗ", Serviceability = Enums.CarServiceability.Serviceable},
                new Car {Number = "А 258 НТ", Mark = "МАЗ", Serviceability = Enums.CarServiceability.Serviceable}
            };

            foreach (var car in cars)
            {
                dataStore.Save(car);
            }

            if (!dataStore.GetAll<ContainerType>().Any())
            {
                ContainerTypeService.CreateTestData(dataStore);
            }

            var dict = new Dictionary<string, decimal[]>
            {
                {"Р 261 ТК", new[] {8m}},
                {"С 013 НЕ", new[] {8m}},
                {"Х 773 КМ", new[] {13m, 15m, 20m}},
                {"А 509 ВН", new[] {13m, 15m, 20m}},
                {"А 258 НТ", new[] {13m, 15m, 20m}},
            };

            foreach (var kvp in dict)
            {
                var car = dataStore.GetAll<Car>().FirstOrDefault(x => x.Number == kvp.Key);
                var containerTypes = dataStore.GetAll<ContainerType>()
                    .Where(x => kvp.Value.Contains(x.Capacity))
                    .ToList();

                if (car == null || !containerTypes.Any())
                {
                    continue;
                }

                foreach (var containerType in containerTypes)
                {
                    var carContainerType = new CarContainerType
                    {
                        Car = car,
                        ContainerType = containerType
                    };
                    dataStore.Save(carContainerType);
                }
            }
        }
    }
}
