using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Core.DataAccess.Params;
using Domain.Core.Exceptions;
using Domain.Dictionary.Customers.Entities;
using Domain.Dictionary.Customers.Interfaces;
using Domain.Dictionary.Customers.Models;
using Domain.Core.Positions.Interfaces;

namespace Domain.Dictionary.Customers.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDataStore dataStore;
        private readonly IPositionService positionService;

        public CustomerService(IDataStore dataStore, IPositionService positionService)
        {
            this.dataStore = dataStore;
            this.positionService = positionService;
        }

        public List<CustomerGetModel> GetAllCustomerModels(StoreLoadParams storeLoadParams)
        {
            return dataStore.GetAll<Customer>()
                .Select(CustomerGetModel.ProjectionExpression)
                .Paging(storeLoadParams)
                .ToList();
        }

        public CustomerGetModel GetCustomerModel(long id)
        {
            var customer = dataStore.Get<Customer>(id);

            return customer != null
                ? new CustomerGetModel(customer)
                : null;
        }

        public Customer Create(CustomerSaveModel customerModel)
        {
            var customer = new Customer();

            customerModel.ApplyToEntity(customer,dataStore,positionService);

            dataStore.Save(customer);

            return customer;
        }

        public async Task<Customer> CreateAsync(CustomerSaveModel customerModel)
        {
            var customer = new Customer();

            customerModel.ApplyToEntity(customer, dataStore, positionService);

            await dataStore.SaveAsync(customer);

            return customer;
        }

        public void Update(long id, CustomerSaveModel customerModel)
        {
            var customer = dataStore.Get<Customer>(id);

            if (customer == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Customer).Name} c идентификатором {id} не существует");
            }

            customerModel.ApplyToEntity(customer, dataStore, positionService);

            dataStore.SaveChanges();
        }

        public async Task UpdateAsync(long id, CustomerSaveModel customerModel)
        {
            var customer = dataStore.Get<Customer>(id);

            if (customer == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Customer).Name} c идентификатором {id} не существует");
            }

            customerModel.ApplyToEntity(customer, dataStore, positionService);

            await dataStore.SaveChangesAsync();
        }

        public void Delete(long id)
        {
            var customer = dataStore.Get<Customer>(id);

            if (customer == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Customer).Name} c идентификатором {id} не существует");
            }

            dataStore.Delete(customer);
        }

        public async Task DeleteAsync(long id)
        {
            var customer = dataStore.Get<Customer>(id);

            if (customer == null)
            {
                throw new EntityNotFoundException($"Запись типа {typeof(Customer).Name} c идентификатором {id} не существует");
            }
            await dataStore.DeleteAsync(customer);
        }

        public static void CreateTestData(IDataStore dataStore)
        {
            if (!Settings.TestEnvironment || dataStore.GetAll<Customer>().Any())
            {
                return;
            }

            var customers = new[]
            {
                new Customer { Inn = "5402536016",
                    Name = "ООО «МираСтиль»",
                    Description = "Оптовая продажа обуви",
                    Phone = "+7(962) 552-42-21",
                    Address = "Химград",
                ContactPersonName ="Вероника",
                ContactPersonPhone ="+7(962) 552-42-21"},
                new Customer { Inn = "",
                    Name = "8 детская больница",
                    Description = "",
                    Phone = "+7(966) 260-79-40",
                    Address = "Галеева 11",
                ContactPersonName ="неизвестно",
                ContactPersonPhone ="+7(966) 260-79-40"}
            };

            foreach (var customer in customers)
            {
                dataStore.Save(customer);
            }
        }
    }
}
