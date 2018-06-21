using System.Collections.Generic;
using Core.DataAccess.Params;
using Domain.Dictionary.Cars.Models;
using System.Threading.Tasks;
using Domain.Core.Positions.Models;
using Domain.Dictionary.ContainerTypes.Models;

namespace Domain.Dictionary.Cars.Interfaces
{
    public interface ICarService
    {
        List<CarGetModel> GetAllCarModels(StoreLoadParams storeLoadParams);

        CarGetModel GetCarModel(long id);

        List<CarContainerTypeGetModel> GetAllCarContainerTypeModels(long carId, StoreLoadParams storeLoadParams);
        List<ContainerTypeGetModel> GetAvailableContainerTypeModels(long carId, StoreLoadParams storeLoadParams);

        void Create(CarSaveModel carModel);
        Task CreateAsync(CarSaveModel carModel);

        void Update(long id, CarSaveModel carModel);
        Task UpdateAsync(long id, CarSaveModel carModel);

        void UpdatePosition(long id, PositionModel positionModel);
        Task UpdatePositionAsync(long id, PositionModel positionModel);

        void Delete(long id);
        Task DeleteAsync(long id);

        void AddCarContainerType(long id, long containerTypeId);
        Task AddCarContainerTypeAsync(long id, long containerTypeId);

        void DeleteCarContainerType(long id);
        Task DeleteCarContainerTypeAsync(long id);
    }
}
