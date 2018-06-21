using System.Web.Mvc;
using Core.DataAccess.Params;
using Domain.Dictionary.ContainerTypes.Interfaces;
using Domain.Dictionary.ContainerTypes.Models;
using Tbo.WebHost.Controllers.MVC.Common;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    /// <summary>
    /// Типы контейнеров
    /// </summary>
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class ContainerTypesController : BaseController
    {
        private readonly IContainerTypeService _containerTypesService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="containerTypesService">Сервис для работы с типами контейнеров</param>
        public ContainerTypesController(IContainerTypeService containerTypesService)
        {
            _containerTypesService = containerTypesService;
        }

        // GET: Containers
        /// <summary>
        /// Получение списка всех элементов
        /// </summary>
        public ActionResult Index()
        {
            var model = _containerTypesService.GetAllContainerTypeModels(null);

            return View(model);
        }

        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Create()
        {
            var model = new ContainerTypeSaveModel();
            return PartialView("Partial/Create", model);
        }

        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Edit(long id)
        {
            var model = _containerTypesService.GetContainerTypeModel(id);
            return PartialView("Partial/Edit", model);
        }
    }
}