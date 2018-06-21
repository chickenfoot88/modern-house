using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.Interfaces;
using Core.Exceptions;
using Domain.Registries.Schedules.Entities;
using Domain.Registries.Schedules.Interfaces;
using Domain.Registries.Schedules.Models;

namespace Domain.Registries.Schedules.Services
{
    internal class SchedulesService : ISchedulesService
    {
        private readonly IDataStore dataStore;

        public SchedulesService(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public List<ScheduleGetModel> GetAllModels()
        {
            return dataStore.GetAll<Schedule>()
                .Select(ScheduleGetModel.ProjectionExpression)
                .ToList();
        }

        public ScheduleGetModel GetModel(long id)
        {
            return dataStore.GetAll<Schedule>()
                .Where(x => x.Id == id)
                .Select(ScheduleGetModel.ProjectionExpression)
                .FirstOrDefault();
        }

        public void Create(ScheduleSaveModel scheduleModel)
        {
            var entity = new Schedule();

            scheduleModel.ApplyToEntity(entity, dataStore);

            BeforeSaveValidate(entity);

            dataStore.Save(entity);
        }

        public async Task CreateAsync(ScheduleSaveModel scheduleModel)
        {
            var entity = new Schedule();

            scheduleModel.ApplyToEntity(entity, dataStore);

            BeforeSaveValidate(entity);

            await dataStore.SaveAsync(entity);
        }

        public void Update(long id, ScheduleSaveModel scheduleModel)
        {
            var entity = dataStore.Get<Schedule>(id);

            scheduleModel.ApplyToEntity(entity, dataStore);

            BeforeSaveValidate(entity);

            dataStore.SaveChanges();
        }

        public async Task UpdateAsync(long id, ScheduleSaveModel scheduleModel)
        {
            var entity = dataStore.Get<Schedule>(id);

            scheduleModel.ApplyToEntity(entity, dataStore);

            BeforeSaveValidate(entity);

            await dataStore.SaveChangesAsync();
        }

        public void Delete(long id)
        {
            var entity = dataStore.Get<Schedule>(id);

            dataStore.Delete(entity);
        }

        public async Task DeleteAsync(long id)
        {
            var entity = dataStore.Get<Schedule>(id);

            await dataStore.DeleteAsync(entity);
        }


        private void BeforeSaveValidate(Schedule entity)
        {
            var schedules = dataStore.GetAll<Schedule>()
                .Select(x => new
                {
                    x.Id,
                    x.Date,
                    x.CarId,
                    x.DriverId
                })
                .ToList();

            var existsForCar = schedules
                .Any(x => x.Date.Date == entity.Date.Date
                && x.CarId == entity.Car.Id
                && x.Id != entity.Id);

            if (existsForCar)
            {
                throw new ValidationException($"График работ на дату \"{entity.Date:dd.MM.yyyy}\" для автомобиля {entity.Car.Mark} {entity.Car.Number} уже составлен");
            }

            var existsForDriver = schedules
                .Any(x => x.Date.Date == entity.Date.Date 
                && x.DriverId == entity.Driver.Id
                && x.Id != entity.Id);

            if (existsForDriver)
            {
                throw new ValidationException($"График работ на дату \"{entity.Date:dd.MM.yyyy}\" для водителя {entity.Driver.Name} уже составлен");
            }
        }
    }
}