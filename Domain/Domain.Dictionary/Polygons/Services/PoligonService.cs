using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Core.DataAccess.Params;
using Domain.Core.Exceptions;
using Domain.Core.Positions.Interfaces;
using Domain.Core.Positions.Models;
using Domain.Core.Positions.Services;
using Domain.Dictionary.Polygons.Entities;
using Domain.Dictionary.Polygons.Interfaces;
using Domain.Dictionary.Polygons.Models;

namespace Domain.Dictionary.Polygons.Services
{
    public class PolygonService : IPolygonService
    {
        private readonly IDataStore dataStore;
        private readonly IPositionService positionService;

        public PolygonService(IDataStore dataStore, PositionService positionService)
        {
            this.dataStore = dataStore;
            this.positionService = positionService;
        }

        public List<PolygonGetModel> GetAllPolygonModels(StoreLoadParams storeLoadParams)
        {
            return dataStore.GetAll<Polygon>()
                .Select(PolygonGetModel.ProjectionExpression)
                .Paging(storeLoadParams)
                .ToList();
        }

        public PolygonGetModel GetPolygonModel(long id)
        {
            var polygon = dataStore.Get<Polygon>(id);

            return polygon != null
                ? new PolygonGetModel(polygon)
                : null;
        }

        public void Create(PolygonSaveModel polygonModel)
        {
            var polygon = new Polygon();

            polygonModel.ApplyToEntity(polygon, positionService);

            dataStore.Save(polygon);
        }

        public async Task CreateAsync(PolygonSaveModel polygonModel)
        {
            var polygon = new Polygon();

            polygonModel.ApplyToEntity(polygon, positionService);

            await dataStore.SaveAsync(polygon);
        }

        public void Update(long id, PolygonSaveModel polygonModel)
        {
            var polygon = dataStore.Get<Polygon>(id);

            if (polygon == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Polygon).Name} c идентификатором {id} не существует");
            }

            polygonModel.ApplyToEntity(polygon, positionService);

            dataStore.SaveChanges();
        }

        public async Task UpdateAsync(long id, PolygonSaveModel polygonModel)
        {
            var polygon = dataStore.Get<Polygon>(id);

            if (polygon == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Polygon).Name} c идентификатором {id} не существует");
            }

            polygonModel.ApplyToEntity(polygon, positionService);

            await dataStore.SaveChangesAsync();
        }

        public void UpdatePosition(long id, PositionModel positionModel)
        {
            var polygon = dataStore.Get<Polygon>(id);

            if (polygon == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Polygon).Name} c идентификатором {id} не существует");
            }

            if (polygon.PositionId.HasValue)
            {
                positionService.Update(polygon.PositionId.Value, positionModel.Latitude, positionModel.Longitude);
            }
            else
            {
                polygon.Position = positionService.Create(positionModel.Latitude, positionModel.Longitude);
                dataStore.SaveChanges();
            }
        }

        public async Task UpdatePositionAsync(long id, PositionModel positionModel)
        {
            var polygon = dataStore.Get<Polygon>(id);

            if (polygon == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Polygon).Name} c идентификатором {id} не существует");
            }

            if (polygon.PositionId.HasValue)
            {
                await positionService.UpdateAsync(polygon.PositionId.Value, positionModel.Latitude, positionModel.Longitude);
            }
            else
            {
                polygon.Position = positionService.Create(positionModel.Latitude, positionModel.Longitude);
                await dataStore.SaveChangesAsync();
            }
        }

        public void Delete(long id)
        {
            var polygon = dataStore.Get<Polygon>(id);

            if (polygon == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Polygon).Name} c идентификатором {id} не существует");
            }

            dataStore.Delete(polygon);
        }

        public async Task DeleteAsync(long id)
        {
            var polygon = dataStore.Get<Polygon>(id);

            if (polygon == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Polygon).Name} c идентификатором {id} не существует");
            }

            await dataStore.DeleteAsync(polygon);
        }

        public static void CreateTestData(IDataStore dataStore)
        {
            if (!Settings.TestEnvironment || dataStore.GetAll<Polygon>().Any())
            {
                return;
            }

            var polygons = new[]
            {
                new Polygon {
                    Number = 1,
                    Name = "Восточный",
                    Description = "Принимает ТБО и строительный мусор",
                    Address = "Самосырово",
                Phone="+7(962) 559-20-37"
                },
                new Polygon {
                    Number = 2,
                    Name = "Возрождение",
                    Description = "Принимает строительный мусор",
                    Address = "Район завода Оргсинтез" ,
                Phone="+7(843) 250-06-00"
                },
                new Polygon {
                    Number = 3,
                    Name = "БАЗА",
                    Description = "Принимает ТБО и строительный мусор",
                    Address = "A.Кутуя 153 В",
                Phone="+7(987) 290-09-99"
                }
            };

            foreach (var polygon in polygons)
            {
                dataStore.Save(polygon);
            }
        }
    }
}
