using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess.Params;
using Domain.Dictionary.Drivers.Models;

namespace Domain.Dictionary.Drivers.Interfaces
{
    public interface IDriverService
    {
        List<DriverGetModel> GetAllDriverModels(StoreLoadParams storeLoadParams);

        DriverGetModel GetDriverModel(long id);

        Task CreateAsync(DriverSaveModel driverModel);

        Task UpdateAsync(long id, DriverSaveModel driverModel);

        void Delete(long id);
        Task DeleteAsync(long id);
    }
}
