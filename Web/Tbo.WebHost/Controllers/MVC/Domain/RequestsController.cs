using System;
using System.Linq;
using System.Web.Mvc;
using Core.Extensions;
using Core.Filtering;
using Domain.Dictionary.Cars.Interfaces;
using Domain.Dictionary.Containers.Interfaces;
using Domain.Dictionary.Drivers.Interfaces;
using Domain.Dictionary.Polygons.Interfaces;
using Domain.Registries.Requests.Enums;
using Domain.Registries.Requests.Interfaces;
using Domain.Registries.Requests.Models;
using Tbo.WebHost.Controllers.MVC.Common;
using Tbo.WebHost.Extensions;
using Tbo.WebHost.Models.Registries.Requests;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    /// <summary>
    /// Контроллер заявок
    /// </summary>
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class RequestsController : BaseController
    {
        private readonly IRequestService _requestService;
        private readonly IContainerService _containerService;
        private readonly ICarService _carService;
        private readonly IDriverService _driverService;
        private readonly IPolygonService _polygonService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="requestService">сервис работы с заявками</param>
        /// <param name="containerService">сервис работы с контейнерами</param>
        /// <param name="carService">сервис работы с автомобилями</param>
        /// <param name="driverService">сервис работы с водителями</param>
        /// <param name="polygonService">сервис работы с полигонами</param>
        public RequestsController(
            IRequestService requestService,
            IContainerService containerService,
            ICarService carService,
            IDriverService driverService,
            IPolygonService polygonService
            )
        {
            this._requestService = requestService;
            this._carService = carService;
            this._containerService = containerService;
            this._driverService = driverService;
            this._polygonService = polygonService;
        }

        /// <summary>
        /// Окно заявок
        /// </summary>
        public ActionResult Index()
        {
            var model = new RequestsIndexModel();

            model.FilterStores = new RequestsIndexModelFilterStores
            {
                RequestTypes = Enum.GetValues(typeof(RequestType))
                    .Cast<RequestType>()
                    .Select(x => new SelectListItem { Value = x.ToString(), Text = x.GetDescription() })
                    .ToList()
                    .AddEmptyElement(),

                ContainerTypes = _requestService
                    .GetDataForFilter(x => new FilterItem { Value = x.Container.ContainerType.Id.ToString(), Text = x.Container.ContainerType.Name })
                    .Select(x => new SelectListItem { Value = x.Value, Text = x.Text })
                    .ToList()
                    .AddEmptyElement(),

                Cars = _requestService
                    .GetDataForFilter(x => new FilterItem { Value = x.Car.Id.ToString(), Text = x.Car.Mark + " " + x.Car.Number })
                    .Select(x => new SelectListItem { Value = x.Value, Text = x.Text })
                    .ToList()
                    .AddEmptyElement(),

                Drivers = _requestService
                    .GetDataForFilter(x => new FilterItem { Value = x.Driver.Id.ToString(), Text = x.Driver.Name })
                    .Select(x => new SelectListItem { Value = x.Value, Text = x.Text })
                    .ToList()
                    .AddEmptyElement(),

                RequestStatuses = Enum.GetValues(typeof(RequestStatus))
                    .Cast<RequestStatus>()
                    .Select(x => new SelectListItem { Value = x.ToString(), Text = x.GetDescription() })
                    .ToList()
                    .AddEmptyElement(),

                IsPaids = Enum.GetValues(typeof(IsPaid))
                    .Cast<IsPaid>()
                    .Select(x => new SelectListItem { Value = x.ToString(), Text = x.GetDescription() })
                    .ToList()
                    .AddEmptyElement(),
            };

            return View(model);
        }


        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Create()
        {
            var model = new RequestSaveModel
            {
                CreateDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
            };

            var editWindowModel = new RequestEditWindowModel<RequestSaveModel>(model);

            PrepareEditWindowModel(editWindowModel);
            PrepareCreateWindowModel(editWindowModel);

            return PartialView("Partial/Create", editWindowModel);
        }

        /// <summary>
        /// Частичное представление - открытие окна копирования 
        /// </summary>
        public ActionResult Copy(long id)
        {
            var model = _requestService.GetRequestModelForCopy(id);

            var editWindowModel = new RequestEditWindowModel<RequestSaveModel>(model);

            PrepareEditWindowModel(editWindowModel);

            return PartialView("Partial/Copy", editWindowModel);
        }

        /// <summary>
        /// Частичное представление - открытие окна редактирования 
        /// </summary>
        public ActionResult Edit(long id)
        {
            var model = _requestService.GetRequestModel(id);

            var editWindowModel = new RequestEditWindowModel<RequestGetModel>(model);

            PrepareEditWindowModel(editWindowModel);

            return PartialView("Partial/Edit", editWindowModel);
        }

        private void PrepareEditWindowModel<T>(RequestEditWindowModel<T> editWindowModel) where T:class
        {
            editWindowModel.Stores = new RequestEditWindowStoresModel();

            editWindowModel.Stores.Customers = _requestService.GetAllOrderedCustomerModels(null)
                .Select(x => new SelectListItem {Value = x.Id.ToString(), Text = $"{x.Name}, {x.ContactPersonPhone}"})
                .ToList()
                .AddEmptyElement("Ввести вручную");

            editWindowModel.Stores.Cars = _carService.GetAllCarModels(null)
                .Select(x => new SelectListItem {Value = x.Id.ToString(), Text = $"{x.Mark} {x.Number}"})
                .ToList()
                .AddEmptyElement();

            editWindowModel.Stores.Containers = _containerService.GetAllContainerModels(null)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = $"{x.Number} ({x.ContainerType.Name})" })
                .ToList()
                .AddEmptyElement();

            editWindowModel.Stores.Drivers = _driverService.GetAllDriverModels(null)
                .Select(x => new SelectListItem {Value = x.Id.ToString(), Text = x.Name})
                .ToList()
                .AddEmptyElement();

            editWindowModel.Stores.Polygons = _polygonService.GetAllPolygonModels(null)
                .Select(x => new SelectListItem {Value = x.Id.ToString(), Text = x.Name})
                .ToList()
                .AddEmptyElement();

            editWindowModel.Stores.RequestStatuses = Enum.GetValues(typeof(RequestStatus))
                .Cast<RequestStatus>()
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.GetDescription() })
                .ToList();

            editWindowModel.Stores.RequestTypes = Enum.GetValues(typeof(RequestType))
                .Cast<RequestType>()
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.GetDescription() })
                .ToList();

            editWindowModel.Stores.PaymentTypes = Enum.GetValues(typeof(PaymentType))
                .Cast<PaymentType>()
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.GetDescription() })
                .ToList();
        }

        private void PrepareCreateWindowModel<T>(RequestEditWindowModel<T> editWindowModel) where T : class
        {
            editWindowModel.Stores.Containers = _containerService.GetAllFreeContainerModels(null)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = $"{x.Number} ({x.ContainerType.Name})" })
                .ToList()
                .AddEmptyElement();
        }
    }
}