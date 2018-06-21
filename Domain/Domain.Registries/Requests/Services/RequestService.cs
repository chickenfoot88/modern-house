using System;
using System.Collections.Generic;
using Domain.Registries.Requests.Entities;
using Domain.Registries.Requests.Interfaces;
using Domain.Registries.Requests.Models;
using Core.DataAccess.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.DataAccess.Params;
using Domain.Core.Exceptions;
using Domain.Core.Positions.Interfaces;
using Domain.Dictionary.Customers.Interfaces;
using Domain.Dictionary.Customers.Models;
using Domain.Registries.Requests.Enums;
using Domain.Dictionary.Containers.Entities;
using Core.DataAccess.Extensions;
using Core.Filtering;
using System.Linq.Expressions;

namespace Domain.Registries.Requests.Services
{
    public class RequestService : IRequestService
    {
        private readonly IDataStore _dataStore;
        private readonly IPositionService _positionService;
        private readonly ICustomerService _customerService;

        public RequestService(IDataStore dataStore, IPositionService positionService, ICustomerService customerService)
        {
            _dataStore = dataStore;
            _positionService = positionService;
            _customerService = customerService;
        }

        public List<RequestGetModel> GetAllModels()
        {
            return _dataStore.GetAll<Request>()
                .Select(RequestGetModel.ProjectionExpression)
                .OrderByDescending(x => x.PlannedDateTime)
                .ToList();
        }

        public List<FilterItem> GetDataForFilter(Expression<Func<Request, FilterItem>> selector)
        {
            return _dataStore.GetAll<Request>()
                .Select(selector)
                .ToList()
                .GroupBy(x => x.Value)
                .Where(x => !string.IsNullOrEmpty(x.Key))
                .OrderBy(x => x.Count())
                .Select(g => g.First())
                .ToList();
        }

        public IQueryable<RequestGetModel> GetAllFilteredQuery(FilteredStoreLoadParams<RequestListFilters> loadParams)
        {
            var baseQuery = _dataStore.GetAll<Request>();

            if (loadParams.Filter != null)
            {
                baseQuery = baseQuery
                    //костыль. так ка кзаказчик хочет фильтровать по типу "малые" и "большие", поэтому эти критерии заведены как отрицательные значения
                    .WhereIf(loadParams.Filter.ContainerTypeId.HasValue && loadParams.Filter.ContainerTypeId > 0,
                        x => x.Container.ContainerType.Id == loadParams.Filter.ContainerTypeId.Value)
                    //Малые (6,8)
                    .WhereIf(loadParams.Filter.ContainerTypeId.HasValue && loadParams.Filter.ContainerTypeId == -1,
                        x => x.Container.ContainerType.Capacity <= 8)
                    //Большие (более 8)
                    .WhereIf(loadParams.Filter.ContainerTypeId.HasValue && loadParams.Filter.ContainerTypeId == -2,
                        x => x.Container.ContainerType.Capacity > 8)
                    .WhereIf(loadParams.Filter.CarId.HasValue,
                        x => x.Car.Id == loadParams.Filter.CarId.Value)
                    .WhereIf(loadParams.Filter.DriverId.HasValue,
                        x => x.Driver.Id == loadParams.Filter.DriverId.Value)
                    .WhereIf(loadParams.Filter.DateStart.HasValue,
                        x => x.PlannedDateTime > loadParams.Filter.DateStart.Value)
                    .WhereIf(loadParams.Filter.DateEnd.HasValue,
                        x => x.PlannedDateTime < loadParams.Filter.DateEnd.Value)
                    .WhereIf(!string.IsNullOrEmpty(loadParams.Filter.CustomerFilter),
                        x => x.Customer.Name.ToLower().Contains(loadParams.Filter.CustomerFilter.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(loadParams.Filter.AddressFilter),
                        x => x.Address.ToLower().Contains(loadParams.Filter.AddressFilter.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(loadParams.Filter.ContactPersonPhoneFilter),
                        x => x.ContactPersonPhone.ToLower()
                            .Contains(loadParams.Filter.ContactPersonPhoneFilter.ToLower()))
                    .WhereIf(loadParams.Filter.IsPaids.HasValue,
                        x => x.IsPaid == loadParams.Filter.IsPaids)
                    .WhereIf(loadParams.Filter.RequestId.HasValue,
                        x => x.Id == loadParams.Filter.RequestId)

                    //тип заявки
                    .WhereIf(
                        loadParams.Filter.RequestType.HasValue &&
                        loadParams.Filter.RequestType.Value == RequestType.Install,

                        x => x.Type == RequestType.Install
                    )
                    .WhereIf(
                        loadParams.Filter.RequestType.HasValue &&
                        loadParams.Filter.RequestType.Value == RequestType.Uninstall,

                        x => x.Type == RequestType.Uninstall
                    )


                    //статус заявки
                    .WhereIf(
                        loadParams.Filter.RequestStatus.HasValue &&
                        loadParams.Filter.RequestStatus.Value == RequestStatus.New,

                        x => x.Status == RequestStatus.New
                    )
                    .WhereIf(
                        loadParams.Filter.RequestStatus.HasValue &&
                        loadParams.Filter.RequestStatus.Value == RequestStatus.InWork,

                        x => x.Status == RequestStatus.InWork
                    )
                    .WhereIf(
                        loadParams.Filter.RequestStatus.HasValue &&
                        loadParams.Filter.RequestStatus.Value == RequestStatus.Done,

                        x => x.Status == RequestStatus.Done
                    )
                    .WhereIf(
                        loadParams.Filter.RequestStatus.HasValue &&
                        loadParams.Filter.RequestStatus.Value == RequestStatus.Rejected,

                        x => x.Status == RequestStatus.Rejected
                    );
            }

            var query = baseQuery
                .Select(RequestGetModel.ProjectionExpression)
                .OrderByDescending(x => x.PlannedDateTime);

            return query;
        }

        public List<CustomerGetModel> GetAllOrderedCustomerModels(StoreLoadParams storeLoadParams)
        {
            var customers = _customerService.GetAllCustomerModels(null);
            var counts = _dataStore.GetAll<Request>()
                .Where(x => x.Customer != null)
                .GroupBy(x => x.CustomerId)
                .Select(x => new OrderModel
                {
                    CustomerId = x.Key.Value,
                    Count = x.Count()
                })
                .ToList();

            counts.AddRange(
                customers
                    .Select(x => x.Id)
                    .Except(counts.Select(x => x.CustomerId))
                    .Select(
                        x => new OrderModel
                        {
                            CustomerId = x,
                            Count = 0
                        }
                    )
            );

            var orderedData = customers.Join(counts, x => x.Id, x => x.CustomerId,
                    (customer, count) => new
                    {
                        Customer = customer,
                        count.Count
                    })
                .OrderByDescending(x => x.Count)
                .Select(x => x.Customer)
                .ToList();

            return orderedData;
        }

        private class OrderModel
        {
            public long CustomerId { get; set; }
            public int Count { get; set; }
        }

        public RequestGetModel GetRequestModel(long id)
        {
            var request = _dataStore.Get<Request>(id);

            return request != null
                ? new RequestGetModel(request)
                : null;
        }

        public RequestSaveModel GetRequestModelForCopy(long id)
        {
            var request = _dataStore.Get<Request>(id);

            return request != null
                ? RequestSaveModel.FromEntity(request)
                : null;
        }

        public void Create(RequestSaveModel requestModel)
        {
            var request = new Request();

            requestModel.ApplyToEntity(request, _dataStore, _positionService, _customerService);

            _dataStore.Save(request);

            if (request.ContainerId.HasValue && request.Type == RequestType.Install)
            {
                var container = _dataStore.Get<Container>(request.ContainerId.Value);
                container.Status = Dictionary.Containers.Enums.ContainerStatus.Installed;
                _dataStore.SaveChanges();
            }
        }

        public async Task CreateAsync(RequestSaveModel requestModel)
        {
            var request = new Request
            {
                CreateDateTime = DateTime.Now
            };

            requestModel.ApplyToEntity(request, _dataStore, _positionService, _customerService);

            await _dataStore.SaveAsync(request);

            if (request.ContainerId.HasValue && request.Type == RequestType.Install)
            {
                var container = _dataStore.Get<Container>(request.ContainerId.Value);
                container.Status = Dictionary.Containers.Enums.ContainerStatus.Installed;
                await _dataStore.SaveChangesAsync();
            }
        }

        public void Update(long id, RequestSaveModel requestModel)
        {
            var request = _dataStore.Get<Request>(id);

            if (request == null)
            {
                throw new EntityNotFoundException(
                    $"Запись типа {typeof(Request).Name} c идентификатором {id} не существует");
            }

            requestModel.ApplyToEntity(request, _dataStore, _positionService, _customerService);

            _dataStore.SaveChanges();

            if (request.ContainerId.HasValue && request.Type == RequestType.Install)
            {
                var container = _dataStore.Get<Container>(request.ContainerId.Value);
                container.Status = Dictionary.Containers.Enums.ContainerStatus.Installed;
                container.PositionId = request.PositionId;
                _dataStore.SaveChanges();
            }

            if (request.ContainerId.HasValue && request.Type == RequestType.Uninstall)
            {
                var container = _dataStore.Get<Container>(request.ContainerId.Value);
                container.Status = Dictionary.Containers.Enums.ContainerStatus.Free;
                container.PositionId = null;
                _dataStore.SaveChanges();
            }
        }

        public async Task UpdateAsync(long id, RequestSaveModel requestModel)
        {
            var request = _dataStore.Get<Request>(id);

            if (request == null)
            {
                throw new EntityNotFoundException(
                    $"Запись типа {typeof(Request).Name} c идентификатором {id} не существует");
            }

            //если заявка с типом "Постановка" переходит в статус "Выполнена",
            //то создается заявка с типом "Забор" на соответствующее время
            if (request.Type == RequestType.Install
                && request.Status != RequestStatus.Done
                && requestModel.Status == RequestStatus.Done)
            {
                //"малые" контейнеры - забор через 3 дня, "большие" - через день
                var daysCountForUninstall = request.Container?.ContainerType.Capacity <= 8 ? 3 : 1;

                var requestForUninstall = new Request
                {
                    Status = RequestStatus.New,
                    Type = RequestType.Uninstall,
                    Comment = $"Автоматически созданная заявка на забор на основании заявки номер '{request.Id}'.",
                    CreateDateTime = DateTime.Now,
                    IsPaid = IsPaid.No,
                    Sum = 0,
                    Address = request.Address,
                    ContactPersonName = request.ContactPersonName,
                    ContactPersonPhone = request.ContactPersonPhone,
                    ContainerId = request.ContainerId,
                    CustomerId = request.CustomerId,
                    DriverId = request.DriverId,
                    CarId = request.CarId,
                    PaymentType = request.PaymentType,
                    PlannedDateTime = request.PlannedUninstallDateTime ??
                                      (request.PlannedDateTime.AddDays(daysCountForUninstall) < DateTime.Now
                                          ? DateTime.Now
                                          : request.PlannedDateTime.AddDays(daysCountForUninstall)),
                    PolygonId = request.PolygonId,
                    PositionId = request.PositionId
                };

                await _dataStore.SaveAsync(requestForUninstall);
            }

            requestModel.ApplyToEntity(request, _dataStore, _positionService, _customerService);

            await _dataStore.SaveChangesAsync();

            //устанавливаем адрес и местоположение контейнера, равного местоположению заявки
            if (request.ContainerId.HasValue && request.Type == RequestType.Install)
            {
                var container = _dataStore.Get<Container>(request.ContainerId.Value);
                container.Status = Dictionary.Containers.Enums.ContainerStatus.Installed;
                container.PositionId = request.PositionId;
                container.Address = request.Address;
                await _dataStore.SaveChangesAsync();
            }
            //освобождаем контейнер, убираем адрес и местоположение
            if (request.ContainerId.HasValue && request.Type == RequestType.Uninstall)
            {
                var container = _dataStore.Get<Container>(request.ContainerId.Value);
                container.Status = Dictionary.Containers.Enums.ContainerStatus.Free;
                container.PositionId = null;
                container.Address = null;
                await _dataStore.SaveChangesAsync();
            }
        }

        public void Delete(long id)
        {
            var request = _dataStore.Get<Request>(id);

            if (request == null)
            {
                throw new EntityNotFoundException(
                    $"Запись типа {typeof(Request).Name} c идентификатором {id} не существует");
            }

            _dataStore.Delete(request);

            if (request.ContainerId.HasValue)
            {
                var container = _dataStore.Get<Container>(request.ContainerId.Value);
                container.Status = Dictionary.Containers.Enums.ContainerStatus.Free;
                _dataStore.SaveChanges();
            }
        }

        public async Task DeleteAsync(long id)
        {
            var request = _dataStore.Get<Request>(id);

            if (request == null)
            {
                throw new EntityNotFoundException(
                    $"Запись типа {typeof(Request).Name} c идентификатором {id} не существует");
            }

            await _dataStore.DeleteAsync(request);

            if (request.ContainerId.HasValue)
            {
                var container = _dataStore.Get<Container>(request.ContainerId.Value);
                container.Status = Dictionary.Containers.Enums.ContainerStatus.Free;
                await _dataStore.SaveChangesAsync();
            }
        }

        public void CreateTestData(IDataStore dataStore)
        {
            if (!Settings.TestEnvironment || dataStore.GetAll<Request>().Any())
            {
                return;
            }

            Dictionary.Containers.Services.ContainerService.CreateTestData(dataStore);
            var firstContainer = dataStore.GetAll<Container>().FirstOrDefault();

            Dictionary.Cars.Services.CarService.CreateTestData(dataStore);

            Dictionary.Drivers.Services.DriverService.CreateTestData(dataStore);

            var firstCustomer = dataStore.GetAll<Dictionary.Customers.Entities.Customer>().FirstOrDefault();
            var secondCustomer = dataStore.GetAll<Dictionary.Customers.Entities.Customer>().Skip(1).FirstOrDefault();

            var requests = new[]
            {
                new Request
                {
                    CreateDateTime = new DateTime(2017, 03, 12),
                    Address = "Восстания, 32",
                    Container = firstContainer,
                    Sum = 2000,
                    IsPaid = IsPaid.Yes,
                    Status = RequestStatus.InWork,
                    Type = RequestType.Install,
                    Customer = firstCustomer
                },
                new Request
                {
                    CreateDateTime = new DateTime(2017, 08, 11),
                    Address = "Восстания, 32",
                    Container = firstContainer,
                    Sum = 2000,
                    IsPaid = IsPaid.Yes,
                    Status = RequestStatus.InWork,
                    Type = RequestType.Uninstall,
                    Customer = secondCustomer
                }
            };

            foreach (var request in requests)
            {
                dataStore.Save(request);
            }
        }
    }
}
