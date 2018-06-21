using System.Web.Mvc;
using Core.DataAccess.Params;
using Domain.Dictionary.Polygons.Interfaces;
using Domain.Dictionary.Polygons.Models;
using Tbo.WebHost.Controllers.MVC.Common;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    /// <summary>
    /// Контоллер для операций с полигонами
    /// </summary>
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class PolygonsController : BaseController
    {
        private readonly IPolygonService _polygonService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="polygonService">Сервис для работы с автомобилями</param>
        public PolygonsController(IPolygonService polygonService)
        {
            this._polygonService = polygonService;
        }

        // GET: Polygons
        public ActionResult Index(int page = 1, int countOnPage = 10)
        {
            var storeLoadParams = new StoreLoadParams
            {
                Start = page - 1,
                Length = countOnPage
            };

            var model = _polygonService.GetAllPolygonModels(storeLoadParams);

            return View(model);
        }

        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Create()
        {
            var model = new PolygonSaveModel();
            return PartialView("Partial/Create", model);
        }

        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Edit(long id)
        {
            var model = _polygonService.GetPolygonModel(id);
            return PartialView("Partial/Edit", model);
        }
    }
}