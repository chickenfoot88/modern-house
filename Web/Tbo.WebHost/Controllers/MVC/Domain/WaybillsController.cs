using System.Linq;
using System.Web.Mvc;
using Core.DataAccess.Extensions;
using Domain.Dictionary.Cars.Interfaces;
using Domain.Registries.Requests.Interfaces;
using Domain.Registries.Waybills.Interfaces;
using Domain.Registries.Waybills.Models;
using Tbo.WebHost.Controllers.MVC.Common;
using Tbo.WebHost.Models.Registries.Waybills;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    /// <summary>
    /// Контроллер маршрутных листов
    /// </summary>
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class WaybillsController : BaseController
    {
        private readonly IWaybillService _waybillService;
        private readonly ICarService _carService;
        private readonly IRequestService _requestService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="waybillService">сервис маршрутных листов</param>
        /// <param name="carService">сервис автомобилей</param>
        public WaybillsController(IWaybillService waybillService, ICarService carService, IRequestService requestService)
        {
            _waybillService = waybillService;
            _carService = carService;
            _requestService = requestService;
        }

        // GET: Waybills
        public ActionResult Index()
        {
            var list = _waybillService.GetAllWaybillModels(null);

            return View(list);
        }

        public ActionResult Create()
        {
            var model = new WaybillSaveModel();

            var cars = _carService.GetAllCarModels(null)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = $"{x.Mark} {x.Number}"
                })
                .ToList();

            var editWindowModel = new WaybillsEditWindowModel<WaybillSaveModel>(model, cars);

            return View("Partial/Create", editWindowModel);
        }


        // GET: Waybills/Edit
        public ActionResult Edit(long id)
        {
            var model = _waybillService.GetWaybillModel(id);
            
            return View(model);
        }

        public ActionResult CreateWaybillRequest(long waybillId)
        {
            var waybill = _waybillService
                .GetWaybillModel(waybillId);

            var usedRequestIds = _waybillService
                .GetAllWaybillRequestModels(waybillId, null)
                .Select(x => x.Request.Id);

            var requests = _requestService.GetAllModels()
                .Where(x => !usedRequestIds.Contains(x.Id))
                .WhereIf(waybill?.WaybillDateTime != null,
                    x => x.PlannedDateTime.Date == waybill.WaybillDateTime.Date)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Address })
                .ToList();

            var model = new WaybillRequestSaveModel
            {
                WaybillId = waybillId.ToString()
            };

            var editWindowModel = new WaybillsRequestsEditWindowModel<WaybillRequestSaveModel>(model, requests);


            return View("Partial/CreateWaybillRequest", editWindowModel);
        }
    }
}