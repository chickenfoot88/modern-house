using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.MobileApi.Interfaces;
using Domain.MobileApi.Models;
using Microsoft.AspNet.Identity;
using Tbo.WebHost.Controllers.Api.Common;

namespace Tbo.WebHost.Controllers.MobileApi.Requests
{
    /// <summary>
    /// Контоллер для операций с заявками
    /// </summary>
    [RoutePrefix("api/mobile/requests")]
    [Authorize(Roles = "Driver", Users = "")]
    public class RequestsApiController : BaseApiController
    {
        private readonly IDriverRequestsService _driverRequestsService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        public RequestsApiController(IDriverRequestsService driverRequestsService)
        {
            this._driverRequestsService = driverRequestsService;
        }

        /// <summary>
        /// Получить список заявок
        /// </summary>
        [Route("")]
        [Attributes.ResponseType(typeof(List<RequestListModel>))]
        public HttpResponseMessage GetList()
        {
            var id = User.Identity.GetUserId<long>();
            var result = _driverRequestsService.GetList(id, DateTime.Today);
            return Success(result);
        }

        /// <summary>
        /// Получить заявку по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        [Route("{id}")]
        [Attributes.ResponseType(typeof(RequestGetModel))]
        public HttpResponseMessage Get(long id)
        {
            var result = _driverRequestsService.GetById(id);
            if (result == null)
            {
                return Failure("Заявка с указанным идентификатором не найден");
            }

            return Success(result);
        }

        /// <summary>
        /// Завершить заявку
        /// </summary>
        [Route("{id}/complete")]
        public async Task<HttpResponseMessage> CompleteRequest(long id)
        {
            await _driverRequestsService.CompleteRequest(id);
            return Success();
        }

        /// <summary>
        /// Отклонить заявку
        /// </summary>
        [Route("{id}/reject")]
        public async Task<HttpResponseMessage> RejectRequest(long id)
        {
            await _driverRequestsService.RejectRequest(id);
            return Success();
        }

        /// <summary>
        /// Взять в работу
        /// </summary>
        [Route("{id}/proceed")]
        public async Task<HttpResponseMessage> ProceedRequest(long id)
        {
            await _driverRequestsService.ProceedRequest(id);
            return Success();
        }
    }
}