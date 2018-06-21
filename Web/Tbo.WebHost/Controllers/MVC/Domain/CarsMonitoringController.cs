using Domain.Registries.Requests.Interfaces;
using System.Web.Mvc;
using Tbo.WebHost.Controllers.MVC.Common;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    /// <summary>
    /// Контроллер
    /// </summary>

    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class CarsMonitoringController : BaseController
    {
        private readonly IMonitoringService monitoringService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        public CarsMonitoringController(IMonitoringService monitoringService)
        {
            this.monitoringService = monitoringService;
        }

        // GET: CarsMonitoring
        public ActionResult Index()
        {
            var positions = monitoringService.GetAllCarsPosition();

            return View(positions);
        }
    }
}