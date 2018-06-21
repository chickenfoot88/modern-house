using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.MobileApi.Interfaces;
using Domain.MobileApi.Models;
using Core.DataAccess.Interfaces;
using Domain.Core.Positions.Interfaces;
using Domain.Dictionary.Drivers.Entities;
using Domain.Registries.Schedules.Entities;

namespace Domain.MobileApi.Services
{
    internal class DriverUserService : IDriverUserService
    {
        private IDataStore _dataStore;
        private IPositionService _positionService;

        public DriverUserService(IDataStore dataStore, IPositionService positionService)
        {
            _dataStore = dataStore;
            _positionService = positionService;
        }
        public DriverUserInfo GetDriverUserInfo(long userId)
        {
            var driver = GetDriverByUserId(userId);

            var result = new DriverUserInfo();
            result.Profile = new DriverUserProfile
            {
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Patronimyc = driver.Patronymic,
                PhoneNumber = driver.PhoneNumber
            };

            result.Car = _dataStore.GetAll<Schedule>()
                .Where(x => x.DriverId == driver.Id)
                .Where(x => x.Date == DateTime.Today)
                .Select(x => new DriverUserCar
                {
                    Mark = x.Car.Mark,
                    Number = x.Car.Number
                })
                .FirstOrDefault();

            return result;
        }

        public async Task UpdateCarPosition(long userId, decimal latitude, decimal longitude)
        {
            var driver = GetDriverByUserId(userId);
            var car = _dataStore.GetAll<Schedule>()
                .Where(x => x.DriverId == driver.Id)
                .Where(x => x.Date == DateTime.Today)
                .Select(x => x.Car)
                .FirstOrDefault();

            if (car == null)
            {
                await Task.FromResult(0);
                return;
            }

            if (car.Position != null)
            {
                await _positionService.UpdateAsync(car.Position.Id, latitude, longitude);
            }
            else
            {
                car.Position = await _positionService.CreateAsync(latitude, longitude);
                await _dataStore.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Получить сущность "Водитель" по пользователю
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        /// <returns>Водитель</returns>
        private Driver GetDriverByUserId(long userId)
        {
            var driver = _dataStore.GetAll<Driver>()
                .FirstOrDefault(x => x.UserId == userId);

            if (driver == null)
            {
                throw new KeyNotFoundException("По данному пользователю не найдено информации");
            }

            return driver;
        }
    }
}
