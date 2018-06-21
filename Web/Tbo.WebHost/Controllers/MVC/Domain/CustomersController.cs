using System.Linq;
using System.Web.Mvc;
using Domain.Dictionary.Customers.Interfaces;
using Domain.Dictionary.Customers.Models;
using Tbo.WebHost.Controllers.MVC.Common;

namespace Tbo.WebHost.Controllers.MVC.Domain
{
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class CustomersController : BaseController
    {
        private readonly ICustomerService customerService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="customerService">Сервис для работы с заказчиками</param>
        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        // GET: Customers
        public ActionResult Index()
        {
            var result = customerService.GetAllCustomerModels(null);
            return View(result);
        }

        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Create()
        {
            var model = new CustomerSaveModel();
            return PartialView("Partial/Create", model);
        }

        /// <summary>
        /// Частичное представление - открытие окна создания 
        /// </summary>
        public ActionResult Edit(long id)
        {
            var model = customerService.GetCustomerModel(id);
            return PartialView("Partial/Edit", model);
        }
    }
}