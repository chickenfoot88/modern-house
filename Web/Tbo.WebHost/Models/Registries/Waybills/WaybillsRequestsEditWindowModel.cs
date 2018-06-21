using System.Collections.Generic;
using System.Web.Mvc;

namespace Tbo.WebHost.Models.Registries.Waybills
{
    /// <summary>
    /// Модель для открытия окна создания/редактирвоания заявки
    /// </summary>
    /// <typeparam name="T">тип dto</typeparam>
    public class WaybillsRequestsEditWindowModel<T> where T:class
    {
        /// <summary>
        /// Данные
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Сторы для DropDownList-ов
        /// </summary>
        public WaybillsRequestsEditWindowStoresModel Stores { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">dto</param>
        /// <param name="requests">заявки</param>
        public WaybillsRequestsEditWindowModel(T data, List<SelectListItem> requests)
        {
            Data = data;
            
            Stores = new WaybillsRequestsEditWindowStoresModel
            {
                Requests = requests
            };
        }
    }

    /// <summary>
    /// Модель сторов для DropDownList-ов
    /// </summary>
    public class WaybillsRequestsEditWindowStoresModel
    {
        /// <summary>
        /// Статусы
        /// </summary>
        public List<SelectListItem> Requests { get; set; }
    }
}