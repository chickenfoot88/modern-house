using System.Linq;
using System.Web.Mvc;
using DAL.EF.Implementations;
using Domain.Dictionary.Cars.Interfaces;
using Tbo.WebHost.Controllers.MVC.Common;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class CarContainerTypesController : BaseController
    {
        private readonly ICarService carService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="carService">Сервис для работы с автомобилями</param>
        public CarContainerTypesController(ICarService carService)
        {
            this.carService = carService;
        }

        // GET: Create
        public ActionResult Create(long carId)
        {
            var available = carService.GetAvailableContainerTypeModels(carId, null)
                .Select(x => new SelectListItem
                {
                    Text = $"{x.Name} ({x.Capacity})",
                    Value = x.Id.ToString()
                })
                .ToList();
            return PartialView("Partial/Create", available);
        }

        // GET: Edit
        public ActionResult Edit(long id)
        {
            return PartialView("Partial/Edit");
        }


        // GET: List
        public ActionResult List(long carId)
        {
            var num = 1;
            var model = carService.GetAllCarContainerTypeModels(carId, null)
                .Select(x =>
                {
                    x.Number = num++;
                    return x;
                })
                .ToList();
            return View(model);
        }
    }
}