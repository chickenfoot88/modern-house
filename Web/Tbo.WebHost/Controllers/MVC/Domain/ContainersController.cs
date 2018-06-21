using System;
using System.Linq;
using System.Web.Mvc;
using Core.Extensions;
using Domain.Dictionary.Containers.Enums;
using Domain.Dictionary.Containers.Interfaces;
using Domain.Dictionary.Containers.Models;
using Domain.Dictionary.ContainerTypes.Interfaces;
using Tbo.WebHost.Controllers.MVC.Common;
using Tbo.WebHost.Models.Dictionaries.Containers;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    /// <summary>
    /// Контроллер работы с контейнерами
    /// </summary>
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class ContainersController : BaseController
    {
        private readonly IContainerService _containertService;
        private readonly IContainerTypeService _containerTypeService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="containertService">Сервис для работы с контейнерами</param>
        /// <param name="containerTypeService">Сервис работы с типами контейнеров</param>
        public ContainersController(IContainerService containertService, IContainerTypeService containerTypeService)
        {
            this._containertService = containertService;
            this._containerTypeService = containerTypeService;
        }

        // GET: Containers
        public ActionResult Index()
        {
            var model = _containertService.GetAllContainerModels(null);
            ViewBag.TotalCount = model.Count;
            ViewBag.FreeCount = model.Count(x => x.Status == ContainerStatus.Free);
            ViewBag.InstalledCount = model.Count(x => x.Status == ContainerStatus.Installed);

            ViewBag.CountByType = model
                .GroupBy(x => x.ContainerType?.Name)
                .OrderBy(x => x.Key)
                .Select(x => $" {x.Key}: {x.Sum(s => 1)} шт.")
                .ToList();

            return View(model);
        }

        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Create()
        {
            var model = new ContainerSaveModel();

            var containerStatuses = Enum.GetValues(typeof(ContainerStatus))
                .Cast<ContainerStatus>()
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.GetDescription() })
                .ToList();

            var containerTypes = _containerTypeService.GetAllContainerTypeModels(null)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                .ToList();

            var editWindowModel = new ContainerEditWindowModel<ContainerSaveModel>(model, containerStatuses, containerTypes);

            return PartialView("Partial/Create", editWindowModel);
        }

        /// <summary>
        /// Частичное представление - открытие окна редактирования 
        /// </summary>
        public ActionResult Edit(long id)
        {
            var model = _containertService.GetContainerModel(id);

            var containerStatuses = Enum.GetValues(typeof(ContainerStatus))
                .Cast<ContainerStatus>()
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.GetDescription() })
                .ToList();

            var containerTypes = _containerTypeService.GetAllContainerTypeModels(null)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
                .ToList();

            var editWindowModel = new ContainerEditWindowModel<ContainerGetModel>(model, containerStatuses, containerTypes);

            return PartialView("Partial/Edit", editWindowModel);
        }
    }
}