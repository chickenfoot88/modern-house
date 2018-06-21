using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Core.DataAccess.Params;
using Domain.Core.Exceptions;
using Domain.Registries.Waybills.Entities;
using Domain.Registries.Waybills.Interfaces;
using Domain.Registries.Waybills.Models;

namespace Domain.Registries.Waybills.Services
{
    public class WaybillService : IWaybillService
    {
        private readonly IDataStore _dataStore;

        public WaybillService(IDataStore dataStore)
        {
            this._dataStore = dataStore;
        }

        public List<WaybillGetModel> GetAllWaybillModels(StoreLoadParams loadParams)
        {
            return _dataStore.GetAll<Waybill>()
                .Select(WaybillGetModel.ProjectionExpression)
                .Paging(loadParams)
                .ToList();
        }

        public WaybillGetModel GetWaybillModel(long id)
        {
            var waybill = _dataStore.Get<Waybill>(id);

            return waybill != null
                ? new WaybillGetModel(waybill, _dataStore)
                : null;
        }

        public void Create(WaybillSaveModel waybillModel)
        {
            var waybill = new Waybill();

            waybillModel.ApplyToEntity(waybill, _dataStore);

            _dataStore.Save(waybill);
        }

        public async Task CreateAsync(WaybillSaveModel waybillModel)
        {
            var waybill = new Waybill();

            waybillModel.ApplyToEntity(waybill, _dataStore);

            await _dataStore.SaveAsync(waybill);
        }

        public void Update(long id, WaybillSaveModel waybillModel)
        {
            var waybill = _dataStore.Get<Waybill>(id);

            if(waybill == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Waybill).Name} c идентификатором {id} не существует");
            }

            waybillModel.ApplyToEntity(waybill, _dataStore);

            _dataStore.SaveChanges();
        }

        public async Task UpdateAsync(long id, WaybillSaveModel waybillModel)
        {
            var waybill = _dataStore.Get<Waybill>(id);

            if (waybill == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Waybill).Name} c идентификатором {id} не существует");
            }

            waybillModel.ApplyToEntity(waybill, _dataStore);

            await _dataStore.SaveChangesAsync();
        }

        public void Delete(long id)
        {
            var waybill = _dataStore.Get<Waybill>(id);

            if (waybill == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Waybill).Name} c идентификатором {id} не существует");
            }

            _dataStore.Delete(waybill);
        }

        public async Task DeleteAsync(long id)
        {
            var waybill = _dataStore.Get<Waybill>(id);

            if (waybill == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Waybill).Name} c идентификатором {id} не существует");
            }

            await _dataStore.DeleteAsync(waybill);
        }

        public List<WaybillRequestGetModel> GetAllWaybillRequestModels(long waybillId, StoreLoadParams loadParams)
        {
            return _dataStore.GetAll<WaybillRequest>()
                .Where(x => x.WaybillId == waybillId)
                .Select(WaybillRequestGetModel.ProjectionExpression)
                .Paging(loadParams)
                .ToList();
        }

        public WaybillRequestGetModel GetWaybillRequestModel(long waybillId, long id)
        {
            var entity = _dataStore.GetAll<WaybillRequest>()
                .Where(x => x.WaybillId == waybillId)
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                return null;
            }

            return new WaybillRequestGetModel(entity);
        }

        public void CreateWaybillRequest(WaybillRequestSaveModel model)
        {
            var entity = new WaybillRequest();
            
            model.ApplyToEntity(entity, _dataStore);

            _dataStore.Save(entity);
        }

        public async Task CreateWaybillRequestAsync(WaybillRequestSaveModel model)
        {
            var entity = new WaybillRequest();

            model.ApplyToEntity(entity, _dataStore);

            await _dataStore.SaveAsync(entity);
        }

        public void DeleteWaybillRequest(long id)
        {
            var waybillRequest = _dataStore.Get<WaybillRequest>(id);

            if (waybillRequest == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(WaybillRequest).Name} c идентификатором {id} не существует");
            }

            _dataStore.Delete(waybillRequest);
        }

        public async Task DeleteWaybillRequestAsync(long id)
        {
            var waybillRequest = _dataStore.Get<WaybillRequest>(id);

            if (waybillRequest == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(WaybillRequest).Name} c идентификатором {id} не существует");
            }

            await _dataStore.DeleteAsync(waybillRequest);
        }
    }
}
