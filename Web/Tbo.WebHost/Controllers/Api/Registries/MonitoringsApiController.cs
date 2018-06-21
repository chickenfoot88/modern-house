using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Tbo.WebHost.Controllers.Api.Common;
using Domain.Registries.Requests.Interfaces;
using Domain.Registries.Monitoring.Models;

namespace Tbo.WebHost.Controllers.Api.Registries
{
    /// <summary>
    /// Контоллер
    /// </summary>
    [RoutePrefix("api/monitorings")]
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class MonitoringsApiController : BaseApiController
    {
        private readonly IMonitoringService monitoringService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        public MonitoringsApiController(IMonitoringService monitoringService)
        {
            this.monitoringService = monitoringService;
        }

        /// <summary>
        /// Получить список позиций автомобилей
        /// </summary>
        /// <returns></returns>
        [Route("cars")]
        [ResponseType(typeof(List<CarMonitoringGetModel>))]
        [HttpGet]
        public HttpResponseMessage GetAllCarsPosition()
        {
            var result = monitoringService.GetAllCarsPosition();
            return Success(result);
        }

        /// <summary>
        /// Получить список позиций контейнеров
        /// </summary>
        /// <returns></returns>
        [Route("containers")]
        [ResponseType(typeof(List<ContainerMonitoringGetModel>))]
        [HttpGet]
        public HttpResponseMessage GetAllContainersPosition()
        {
            var result = monitoringService.GetAllContainersPosition();
            return Success(result);
        }
    }
}