using Core.DataAccess.Params;
using Domain.Dictionary.ContainerTypes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Dictionary.ContainerTypes.Interfaces
{
    public interface IContainerTypeService
    {
        List<ContainerTypeGetModel> GetAllContainerTypeModels(StoreLoadParams storeLoadParams);

        ContainerTypeGetModel GetContainerTypeModel(long id);

        void Create(ContainerTypeSaveModel containerTypeModel);
        Task CreateAsync(ContainerTypeSaveModel containerTypeModel);

        void Update(long id, ContainerTypeSaveModel containerTypeModel);
        Task UpdateAsync(long id, ContainerTypeSaveModel containerTypeModel);

        void Delete(long id);
        Task DeleteAsync(long id);
    }
}
