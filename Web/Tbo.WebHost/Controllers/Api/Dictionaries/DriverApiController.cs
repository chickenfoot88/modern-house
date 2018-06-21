using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.Dictionary.Drivers.Interfaces;
using Domain.Dictionary.Drivers.Models;
using Tbo.WebHost.Attributes;
using Tbo.WebHost.Controllers.Api.Common;

namespace Tbo.WebHost.Controllers.Api.Dictionaries
{
    /// <summary>
    /// Контоллер для операций с водителями
    /// </summary>
    [RoutePrefix("api/drivers")]
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class DriverApiController : BaseApiController
    {
        private readonly IDriverService driverService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="driverService">Сервис для работы с водителями</param>
        public DriverApiController(IDriverService driverService)
        {
            this.driverService = driverService;
        }

        /// <summary>
        /// Получить список водителей
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(List<DriverGetModel>))]
        public HttpResponseMessage GetList()
        {
            var result = driverService.GetAllDriverModels(null);
            return Success(result);
        }

        /// <summary>
        /// Получить водителя по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(DriverGetModel))]
        public HttpResponseMessage Get(long id)
        {
            var result = driverService.GetDriverModel(id);

            if (result == null)
            {
                return Failure("Водитель с указанным идентификатором не найден");
            }

            return Success(result);
        }

        /// <summary>
        /// Добавление водителя
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(DriverSaveModel))]
        public async Task<HttpResponseMessage> Post(UnwrapModel<DriverSaveModel> model)
        {
            if (model?.Data != null)
            {
                await driverService.CreateAsync(model.Data);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение водителя
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(DriverSaveModel))]
        public async Task<HttpResponseMessage> Put(long id, UnwrapModel<DriverSaveModel> model)
        {
            if (model?.Data != null)
            {
                await driverService.UpdateAsync(id, model.Data);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Удаление водителя
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(DriverSaveModel))]
        public async Task<HttpResponseMessage> Delete(long id)
        {
            await driverService.DeleteAsync(id);
            return Success();
        }
    }
}