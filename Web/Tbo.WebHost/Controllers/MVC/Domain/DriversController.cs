using System.Web.Mvc;
using Domain.Dictionary.Cars.Interfaces;
using Domain.Dictionary.Drivers.Interfaces;
using Domain.Dictionary.Drivers.Models;
using Tbo.WebHost.Controllers.MVC.Common;
using Tbo.WebHost.Models.Dictionaries.Drivers;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class DriversController : BaseController
    {
        private readonly IDriverService _driverService;
        private readonly ICarService _carService;

        public DriversController(IDriverService driverService, ICarService carService)
        {
            this._driverService = driverService;
            this._carService = carService;
        }


        // GET: Drivers
        public ActionResult Index()
        {
            var list = _driverService.GetAllDriverModels(null);
            return View(list);
        }

        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Create()
        {
            var model = new DriverSaveModel();

            var editWindowModel = new DriverEditWindowModel<DriverSaveModel>(model);

            return PartialView("Partial/Create", editWindowModel);
        }

        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Edit(long id)
        {
            var model = _driverService.GetDriverModel(id);

            var editWindowModel = new DriverEditWindowModel<DriverGetModel>(model);

            return PartialView("Partial/Edit", editWindowModel);
        }
    }
}