using System.Web.Mvc;
using Domain.Dictionary.Cars.Interfaces;
using Domain.Dictionary.Cars.Models;
using Tbo.WebHost.Controllers.MVC.Common;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    /// <summary>
    /// Контоллер для операций с автомобилями
    /// </summary>
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class CarsController : BaseController
    {
        private readonly ICarService carService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="carService">Сервис для работы с автомобилями</param>
        public CarsController(ICarService carService)
        {
            this.carService = carService;
        }

        /// <summary>
        /// Основное представление - список автомобилей
        /// </summary>
        public ActionResult Index()
        {
            var list = carService.GetAllCarModels(null);
            return View(list);
        }

        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Create()
        {
            var model = new CarSaveModel();
            return PartialView("Partial/Create", model);
        }

        /// <summary>
        /// Открытие окна редактирования
        /// </summary>
        public ActionResult Edit(long id)
        {
            var model = carService.GetCarModel(id);
            return View("Edit", model);
        }

        /// <summary>
        /// Открытие окна редактирования
        /// </summary>
        [HttpPost]
        public ActionResult Edit(long id, CarSaveModel updateModel)
        {
            carService.Update(id, updateModel);

            return RedirectToAction("Edit", new { id = id });
        }
    }
}