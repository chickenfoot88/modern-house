using Core.DataAccess.Params;
using Domain.Registries.Requests.Interfaces;
using Domain.Registries.Requests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Tbo.WebHost.Controllers.Api.Common;

namespace Tbo.WebHost.Controllers.Api.Registries
{
    /// <summary>
    /// Контоллер для операций с заявками
    /// </summary>
    [RoutePrefix("api/requests")]
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class RequestApiController : BaseApiController
    {
        private readonly IRequestService requestService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="requestService">Сервис для работы с заявками</param>
        public RequestApiController(IRequestService requestService)
        {
            this.requestService = requestService;
        }

        /// <summary>
        /// Получить список заявок
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(List<RequestGetModel>))]
        public HttpResponseMessage GetList([FromUri]FilteredStoreLoadParams<RequestListFilters> loadParams)
        {
            var query = requestService.GetAllFilteredQuery(loadParams);
            return SuccessListFrom(query, loadParams);
        }

        /// <summary>
        /// Получить заявку по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(RequestGetModel))]
        public HttpResponseMessage Get(long id)
        {
            var result = requestService.GetRequestModel(id);

            if (result == null)
            {
                return Failure("Заявка с указанным идентификатором не найден");
            }

            return Success(result);
        }

        /// <summary>
        /// Добавление заявки
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public async Task<HttpResponseMessage> Post(UnwrapModel<RequestSaveModel> model)
        {
            if (model?.Data != null)
            {
                model.Data.PlannedDate = HttpUtility.UrlDecode(model.Data.PlannedDate);

                await requestService.CreateAsync(model.Data);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение заявки
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Put(long id, UnwrapModel<RequestSaveModel> model)
        {
            if (model?.Data != null)
            {
                model.Data.PlannedDate = HttpUtility.UrlDecode(model.Data.PlannedDate);

                await requestService.UpdateAsync(id, model.Data);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Удаление заявки
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Delete(long id)
        {
            await requestService.DeleteAsync(id);
            return Success();
        }
    }
}