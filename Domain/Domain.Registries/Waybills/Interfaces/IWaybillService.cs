using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess.Params;
using Domain.Registries.Waybills.Models;

namespace Domain.Registries.Waybills.Interfaces
{
    public interface IWaybillService
    {
        List<WaybillGetModel> GetAllWaybillModels(StoreLoadParams loadParams);

        WaybillGetModel GetWaybillModel(long id);

        void Create(WaybillSaveModel waybillModel);
        Task CreateAsync(WaybillSaveModel waybillModel);

        void Update(long id, WaybillSaveModel waybillModel);
        Task UpdateAsync(long id, WaybillSaveModel waybillModel);

        void Delete(long id);
        Task DeleteAsync(long id);



        List<WaybillRequestGetModel> GetAllWaybillRequestModels(long waybillId, StoreLoadParams loadParams);

        WaybillRequestGetModel GetWaybillRequestModel(long waybillId, long id);


        void CreateWaybillRequest(WaybillRequestSaveModel model);
        Task CreateWaybillRequestAsync(WaybillRequestSaveModel model);

        void DeleteWaybillRequest(long id);
        Task DeleteWaybillRequestAsync(long id);

    }
}