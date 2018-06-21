using System.Collections.Generic;
using Domain.Registries.Requests.Models;
using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.Params;
using Domain.Dictionary.Customers.Models;
using Core.Filtering;
using System.Linq.Expressions;
using System;
using Domain.Registries.Requests.Entities;

namespace Domain.Registries.Requests.Interfaces
{
    public interface IRequestService
    {
        IQueryable<RequestGetModel> GetAllFilteredQuery(FilteredStoreLoadParams<RequestListFilters> loadParams);

        List<FilterItem> GetDataForFilter(Expression<Func<Request, FilterItem>> selector);

        List<RequestGetModel> GetAllModels();
        List<CustomerGetModel> GetAllOrderedCustomerModels(StoreLoadParams storeLoadParams);

        RequestGetModel GetRequestModel(long id);
        RequestSaveModel GetRequestModelForCopy(long id);

        void Create(RequestSaveModel requestModel);
        Task CreateAsync(RequestSaveModel requestModel);

        void Update(long id, RequestSaveModel requestModel);
        Task UpdateAsync(long id, RequestSaveModel requestModel);

        void Delete(long id);
        Task DeleteAsync(long id);
    }
}
