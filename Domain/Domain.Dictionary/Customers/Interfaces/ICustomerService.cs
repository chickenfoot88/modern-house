using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataAccess.Params;
using Domain.Dictionary.Customers.Entities;
using Domain.Dictionary.Customers.Models;

namespace Domain.Dictionary.Customers.Interfaces
{
    public interface ICustomerService
    {
        List<CustomerGetModel> GetAllCustomerModels(StoreLoadParams storeLoadParams);

        CustomerGetModel GetCustomerModel(long id);

        Customer Create(CustomerSaveModel carModel);
        Task<Customer> CreateAsync(CustomerSaveModel carModel);

        void Update(long id, CustomerSaveModel carModel);
        Task UpdateAsync(long id, CustomerSaveModel carModel);

        void Delete(long id);
        Task DeleteAsync(long id);
    }
}
