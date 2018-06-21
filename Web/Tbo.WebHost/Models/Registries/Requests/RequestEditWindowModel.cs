using System.Collections.Generic;
using System.Web.Mvc;

namespace Tbo.WebHost.Models.Registries.Requests
{
    /// <summary>
    /// Модель для открытия окна создания/редактирвоания заявки
    /// </summary>
    /// <typeparam name="T">тип dto</typeparam>
    public class RequestEditWindowModel<T> where T:class
    {
        /// <summary>
        /// Данные
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Сторы для DropDownList-ов
        /// </summary>
        public RequestEditWindowStoresModel Stores { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">dto</param>
        public RequestEditWindowModel(T data)
        {
            Data = data;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public RequestEditWindowModel()
        {
        }
    }

    /// <summary>
    /// Модель сторов для DropDownList-ов
    /// </summary>
    public class RequestEditWindowStoresModel
    {
        /// <summary>
        /// Контейнеры
        /// </summary>
        public List<SelectListItem> Containers { get; set; }

        /// <summary>
        /// Автомобили
        /// </summary>
        public List<SelectListItem> Cars { get; set; }

        /// <summary>
        /// Водители
        /// </summary>
        public List<SelectListItem> Drivers { get; set; }

        /// <summary>
        /// Заказчики
        /// </summary>
        public List<SelectListItem> Customers { get; set; }

        /// <summary>
        /// Статусы заявки
        /// </summary>
        public List<SelectListItem> RequestStatuses { get; set; }

        /// <summary>
        /// Типы заявки
        /// </summary>
        public List<SelectListItem> RequestTypes { get; set; }

        /// <summary>
        /// Типы оплаты
        /// </summary>
        public List<SelectListItem> PaymentTypes { get; set; }

        /// <summary>
        /// Полигоны
        /// </summary>
        public List<SelectListItem> Polygons { get; set; }
    }
}