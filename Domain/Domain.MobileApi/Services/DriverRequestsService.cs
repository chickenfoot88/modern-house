using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.Interfaces;
using Core.Exceptions;
using Core.Extensions;
using Domain.Dictionary.Containers.Entities;
using Domain.Dictionary.Drivers.Entities;
using Domain.MobileApi.Interfaces;
using Domain.MobileApi.Models;
using Domain.Registries.Requests.Entities;
using Domain.Registries.Requests.Enums;

namespace Domain.MobileApi.Services
{
    internal class DriverRequestsService : IDriverRequestsService
    {
        private IDataStore _dataStore;

        public DriverRequestsService(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public List<RequestListModel> GetList(long userId, DateTime date)
        {
            var driver = GetDriverByUserId(userId);

            return _dataStore.GetAll<Request>()
                .Where(x => x.DriverId == driver.Id)
                .ToList()
                .Where(x => x.PlannedDateTime.Date == date.Date)
                    .Select(x => new RequestListModel
                    {
                        Id = x.Id,
                        Address = x.Address,
                        Status = x.Status.GetDescription(),
                        Type = x.Type.GetDescription()
                    })
                .ToList();
        }

        public RequestGetModel GetById(long requestId)
        {
            var entity = _dataStore.Get<Request>(requestId);

            if (entity == null)
            {
                throw new ValidationException($"Не найдена информация для Завки с id = {requestId}");
            }

            return new RequestGetModel
            {
                Id = entity.Id,
                ContainerType = entity.Container?.ContainerType?.Name,
                PaymentType = entity.PaymentType.GetDescription(),
                Status = entity.Status.GetDescription(),
                Type = entity.Type.GetDescription(),
                Sum = entity.Sum,
                Customer = new RequestCustomerModel
                {
                    Address = entity.Customer?.Address,
                    ContactPersonName = entity.Customer?.ContactPersonName,
                    ContactPersonPhone = entity.Customer?.ContactPersonPhone,
                    Latitude = entity.Position?.Latitude,
                    Longitude = entity.Position?.Longitude
                },
                Polygon = new RequestPolygonModel
                {
                    Address = entity.Polygon?.Address,
                    Name = entity?.Polygon?.Name,
                    Phone = entity?.Polygon?.Phone
                }
            };
        }
        public async Task CompleteRequest(long requestId)
        {
            var entity = _dataStore.Get<Request>(requestId);

            if (entity == null)
            {
                throw new ValidationException($"Не найдена информация для Завки с id = {requestId}");
            }

            var validationResult = ValidateChangeStatus(entity, RequestStatus.Done);
            if (validationResult)
            {
                entity.Status = RequestStatus.Done;
                entity.ExecutionDateTime = DateTime.Now;
            }

            if (entity.ContainerId.HasValue && entity.Type == RequestType.Uninstall)
            {
                var container = _dataStore.Get<Container>(entity.ContainerId.Value);
                container.Status = Dictionary.Containers.Enums.ContainerStatus.Free;
                await _dataStore.SaveChangesAsync();
            }

            await _dataStore.SaveChangesAsync();
        }

        public async Task RejectRequest(long requestId)
        {
            var entity = _dataStore.Get<Request>(requestId);

            if (entity == null)
            {
                throw new ValidationException($"Не найдена информация для Завки с id = {requestId}");
            }

            var validationResult = ValidateChangeStatus(entity, RequestStatus.Rejected);
            if (validationResult)
            {
                entity.Status = RequestStatus.Rejected;
            }

            await _dataStore.SaveChangesAsync();

            if (entity.ContainerId.HasValue && entity.Type == RequestType.Install)
            {
                var container = _dataStore.Get<Container>(entity.ContainerId.Value);
                container.Status = Dictionary.Containers.Enums.ContainerStatus.Free;
                await _dataStore.SaveChangesAsync();
            }
        }

        public async Task ProceedRequest(long requestId)
        {
            var entity = _dataStore.Get<Request>(requestId);

            if (entity == null)
            {
                throw new ValidationException($"Не найдена информация для Завки с id = {requestId}");
            }

            var validationResult = ValidateChangeStatus(entity, RequestStatus.InWork);
            if (validationResult)
            {
                entity.Status = RequestStatus.InWork;
            }

            await _dataStore.SaveChangesAsync();
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

        /// <summary>
        /// Проверка возможности перехода в статус
        /// </summary>
        /// <param name="request">заявка</param>
        /// <param name="newStatus">новый статус</param>
        /// <returns>
        ///     true - если возможен переход статуса
        ///     false - если не возможен переход
        /// </returns>
        private bool ValidateChangeStatus(Request request, RequestStatus newStatus)
        {
            switch (request.Status)
            {
                case RequestStatus.Done:
                    //Если заявка завершена - то никуда из этого статуса переходить нельзя
                    return newStatus == RequestStatus.Done;
                case RequestStatus.New:
                    //Из "Новой" возможен переход в "Завершена", "Отклонена" или "В работе" 
                    return newStatus == RequestStatus.Done || newStatus == RequestStatus.Rejected || newStatus == RequestStatus.InWork;
                case RequestStatus.InWork:
                    //Из "В работе" возможен переход в "Завершена", "Отклонена"
                    return newStatus == RequestStatus.Done || newStatus == RequestStatus.Rejected;
                case RequestStatus.Rejected:
                    //Если заявка отклонена - то никуда из этого статуса переходить нельзя
                    return newStatus == RequestStatus.Rejected;
                default:
                    throw new NotImplementedException();

            }
        }
    }
}
