using System;
using System.Linq;
using System.Web.Mvc;
using Domain.Dictionary.Cars.Interfaces;
using Domain.Dictionary.Drivers.Interfaces;
using Domain.Registries.Schedules.Interfaces;
using Domain.Registries.Schedules.Models;
using Tbo.WebHost.Controllers.MVC.Common;
using Tbo.WebHost.Extensions;
using Tbo.WebHost.Models.Registries.Schedules;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    /// <summary>
    /// Контроллер графика работ
    /// </summary>
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class SchedulesController : BaseController
    {
        private readonly ICarService _carService;
        private readonly IDriverService _driverService;
        private readonly ISchedulesService _schedulesService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="carService">сервис работы с автомобилями</param>
        /// <param name="driverService">сервис работы с водителями</param>
        /// <param name="schedulesService">сервис работы с графиком работ</param>
        public SchedulesController(
            ICarService carService,
            IDriverService driverService,
            ISchedulesService schedulesService
            )
        {
            this._carService = carService;
            this._driverService = driverService;
            this._schedulesService = schedulesService;
        }

        /// <summary>
        /// Окно графика работ
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Список
        /// </summary>
        public ActionResult List()
        {
            var models = _schedulesService.GetAllModels();
            return PartialView("Partial/List", models);
        }

        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Create()
        {
            var model = new ScheduleSaveModel()
            {
                DateStr = DateTime.Now.ToString("dd.MM.yyyy")
            };

            var editWindowModel = new SchedulesEditWindowModel<ScheduleSaveModel>(model);

            PrepareEditWindowModel(editWindowModel);

            return PartialView("Partial/Create", editWindowModel);
        }

        /// <summary>
        /// Частичное представление - открытие окна редактирования 
        /// </summary>
        public ActionResult Edit(long id)
        {
            var model = _schedulesService.GetModel(id);

            var editWindowModel = new SchedulesEditWindowModel<ScheduleGetModel>(model);

            PrepareEditWindowModel(editWindowModel);

            return PartialView("Partial/Edit", editWindowModel);
        }

        private void PrepareEditWindowModel<T>(SchedulesEditWindowModel<T> editWindowModel) where T:class
        {
            editWindowModel.Stores = new SchedulesEditWindowStoresModel();

            editWindowModel.Stores.Cars = _carService.GetAllCarModels(null)
                .Select(x => new SelectListItem {Value = x.Id.ToString(), Text = $"{x.Mark} {x.Number}"})
                .ToList()
                .AddEmptyElement();

            editWindowModel.Stores.Drivers = _driverService.GetAllDriverModels(null)
                .Select(x => new SelectListItem {Value = x.Id.ToString(), Text = x.Name})
                .ToList()
                .AddEmptyElement();
        }
    }
}