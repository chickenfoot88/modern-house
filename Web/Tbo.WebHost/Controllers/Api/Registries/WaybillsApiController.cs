using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Core.DataAccess.Params;
using Domain.Registries.Waybills.Interfaces;
using Domain.Registries.Waybills.Models;
using Tbo.WebHost.Controllers.Api.Common;

namespace Tbo.WebHost.Controllers.Api.Registries
{
    /// <summary>
    /// Контоллер для операций с заявками
    /// </summary>
    [RoutePrefix("api/waybills")]
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class WaybillsApiController : BaseApiController
    {
        private readonly IWaybillService _waybillsService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="waybillsService">Сервис для работы с Маршрутными листами</param>
        public WaybillsApiController(IWaybillService waybillsService)
        {
            this._waybillsService = waybillsService;
        }

        /// <summary>
        /// Получить список маршрутных листов
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ResponseType(typeof(List<WaybillGetModel>))]
        public HttpResponseMessage List(StoreLoadParams storeLoadParams)
        {
            var result = _waybillsService.GetAllWaybillModels(storeLoadParams).ToList();
            return Success(result);
        }

        /// <summary>
        /// Получить маршрутный лист по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(WaybillGetModel))]
        public HttpResponseMessage Get(long id)
        {
            var result = _waybillsService.GetWaybillModel(id);

            if (result == null)
            {
                return Failure("Маршрутный лист с указанным идентификатором не найден");
            }

            return Success(result);
        }

        /// <summary>
        /// Добавление маршрутного листа
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public HttpResponseMessage Post(UnwrapModel<WaybillSaveModel> model)
        {
            if (model?.Data != null)
            {
                _waybillsService.CreateAsync(model.Data);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение маршрутного листа
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public HttpResponseMessage Put(long id, UnwrapModel<WaybillSaveModel> model)
        {
            if (model?.Data != null)
            {
                _waybillsService.UpdateAsync(id, model.Data);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Удаление маршрутного листа
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public HttpResponseMessage Delete(long id)
        {
            _waybillsService.DeleteAsync(id);
            return Success();
        }







        /// <summary>
        /// Получить список заявок маршрутного листа
        /// </summary>
        /// <returns></returns>
        [Route("{id}/requests")]
        [ResponseType(typeof(List<WaybillRequestGetModel>))]
        [HttpGet]
        public HttpResponseMessage List(long id, StoreLoadParams storeLoadParams)
        {
            var result = _waybillsService.GetAllWaybillRequestModels(id, storeLoadParams);
            return Success(result);
        }

        /// <summary>
        /// Получить маршрутный лист по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор маршрутного листа</param>
        /// <param name="waybillRequestId">Идентификатор заявки</param>
        /// <returns></returns>
        [Route("requests/{waybillRequestId}")]
        [ResponseType(typeof(WaybillRequestGetModel))]
        [HttpGet]
        public HttpResponseMessage Get(long id, long waybillRequestId)
        {
            var result = _waybillsService.GetWaybillRequestModel(id, waybillRequestId);

            if (result == null)
            {
                return Failure("Маршрутный лист с указанным идентификатором не найден");
            }

            return Success(result);
        }

        /// <summary>
        /// Добавление заявки маршрутного листа
        /// </summary>
        /// <returns></returns>
        [Route("requests")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post(UnwrapModel<WaybillRequestSaveModel> model)
        {
            if (model?.Data != null)
            {
                await _waybillsService.CreateWaybillRequestAsync(model.Data);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Удаление маршрутного листа
        /// </summary>
        /// <returns></returns>
        [Route("requests/{waybillRequestId}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteWaybillRequest(long waybillRequestId)
        {
            await _waybillsService.DeleteWaybillRequestAsync(waybillRequestId);
            return Success();
        }
    }
}