using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Core.DataAccess.Params;
using Core.Identity.Services;
using Domain.Core.Exceptions;
using Domain.Dictionary.Drivers.Entities;
using Domain.Dictionary.Drivers.Interfaces;
using Domain.Dictionary.Drivers.Models;

namespace Domain.Dictionary.Drivers.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDataStore _dataStore;
        private readonly ApplicationUserManager _userManager;

        public DriverService(IDataStore dataStore, ApplicationUserManager userManager)
        {
            this._dataStore = dataStore;
            this._userManager = userManager;
        }

        public List<DriverGetModel> GetAllDriverModels(StoreLoadParams storeLoadParams)
        {
            return _dataStore.GetAll<Driver>()
                .Select(DriverGetModel.ProjectionExpression)
                .Paging(storeLoadParams)
                .ToList();
        }

        public DriverGetModel GetDriverModel(long id)
        {
            var driver = _dataStore.Get<Driver>(id);

            return driver != null
                ? new DriverGetModel(driver)
                : null;
        }

        public async Task CreateAsync(DriverSaveModel driverModel)
        {
            var driver = new Driver();

            await driverModel.ApplyToEntity(driver, _dataStore, _userManager);

            await _dataStore.SaveAsync(driver);
        }

        public async Task UpdateAsync(long id, DriverSaveModel driverModel)
        {
            var driver = _dataStore.Get<Driver>(id);

            if (driver == null)
            {
                throw new EntityNotFoundException(
                    $"Запись типа {typeof(Driver).Name} c идентификатором {id} не существует");
            }

            await driverModel.ApplyToEntity(driver, _dataStore, _userManager);

            await _dataStore.SaveChangesAsync();
        }

        public void Delete(long id)
        {
            var driver = _dataStore.Get<Driver>(id);

            if (driver == null)
            {
                throw new EntityNotFoundException(
                    $"Запись типа {typeof(Driver).Name} c идентификатором {id} не существует");
            }

            _dataStore.Delete(driver);
        }

        public async Task DeleteAsync(long id)
        {
            var driver = _dataStore.Get<Driver>(id);

            if (driver == null)
            {
                throw new EntityNotFoundException(
                    $"Запись типа {typeof(Driver).Name} c идентификатором {id} не существует");
            }

            await _dataStore.DeleteAsync(driver);
        }

        public static void CreateTestData(IDataStore dataStore)
        {
            if (!Settings.TestEnvironment || dataStore.GetAll<Driver>().Any())
            {
                return;
            }

            var drivers = new[]
            {
                new Driver {
                    LastName = "Матвеев",
                    FirstName = "Роман",
                    Patronymic = "Викторович",
                    Name = "Матвеев Роман",
                    PhoneNumber="+7(903) 344-13-78",
                    DriverLicenceNumber="16 ТВ 266206"
                },
                new Driver {LastName = "Габдуллин",
                    FirstName = "Раиль",
                    Patronymic = "Набиуллович",
                    Name = "Габдуллин Раиль",
                PhoneNumber="+7(952) 042-44-49",
                    DriverLicenceNumber="16 32 694695"
                },
                new Driver {LastName = "Адельметов",
                    FirstName = "Максим",
                    Patronymic = "Эдуардович",
                    Name = "Адельметов Максим",
                PhoneNumber="+7(960) 032-90-30",
                    DriverLicenceNumber="16 20 504074"
                },
                new Driver {LastName = "Ибрагимов",
                    FirstName = "Азат",
                    Patronymic = "Ильдарович",
                    Name = "Ибрагимов Азат",
                PhoneNumber="+7(917) 298-48-84",
                    DriverLicenceNumber="16 14 881480"
                },
                new Driver {LastName = "Фозулзянов", FirstName = "Талгат", Patronymic = "Габбасович", Name = "Фозулзянов Талгат",
                PhoneNumber="+7(950) 667-95-21",
                    DriverLicenceNumber="16 07 486640"
                }
            };

            foreach (var driver in drivers)
            {
                dataStore.Save(driver);
            }
        }
    }
}
