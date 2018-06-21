using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.Dictionary.Customers.Interfaces;
using Domain.Dictionary.Customers.Models;
using Tbo.WebHost.Attributes;
using Tbo.WebHost.Controllers.Api.Common;

namespace Tbo.WebHost.Controllers.Api.Dictionaries
{
    /// <summary>
    /// Контоллер для операций с заказчиками
    /// </summary>
    [RoutePrefix("api/customers")]
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class CustomerApiController : BaseApiController
    {
        private readonly ICustomerService customerService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="customerService">Сервис для работы с заказчиками</param>
        public CustomerApiController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        /// <summary>
        /// Получить список заказчиков
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(List<CustomerGetModel>))]
        public HttpResponseMessage GetList()
        {
            var result = customerService.GetAllCustomerModels(null);
            return Success(result);
        }

        /// <summary>
        /// Получить заказчика по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(CustomerGetModel))]
        public HttpResponseMessage Get(long id)
        {
            var result = customerService.GetCustomerModel(id);

            return Success(result);
        }

        /// <summary>
        /// Добавление заказчика
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post(CustomerSaveModel model)
        {
            if (model != null)
            {
                await customerService.CreateAsync(model);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение заказчика
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        public async Task<HttpResponseMessage> Put(long id, CustomerSaveModel model)
        {
            if (model != null)
            {
                await customerService.UpdateAsync(id, model);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Удаление заказчика
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Delete(long id)
        {
            await customerService.DeleteAsync(id);
            return Success();
        }
    }
}