using System.Collections.Generic;
using Core.DataAccess.Params;
using Domain.Dictionary.Containers.Models;
using System.Threading.Tasks;
using Domain.Core.Positions.Models;

namespace Domain.Dictionary.Containers.Interfaces
{
    public interface IContainerService
    {
        List<ContainerGetModel> GetAllContainerModels(StoreLoadParams storeLoadParams);

        List<ContainerGetModel> GetAllFreeContainerModels(StoreLoadParams storeLoadParams);

        ContainerGetModel GetContainerModel(long id);

        void Create(ContainerSaveModel containerModel);
        Task CreateAsync(ContainerSaveModel containerModel);

        void Update(long id, ContainerSaveModel containerModel);
        Task UpdateAsync(long id, ContainerSaveModel containerModel);

        void UpdatePosition(long id, PositionModel positionModel);
        Task UpdatePositionAsync(long id, PositionModel positionModel);

        void Delete(long id);
        Task DeleteAsync(long id);
    }
}
