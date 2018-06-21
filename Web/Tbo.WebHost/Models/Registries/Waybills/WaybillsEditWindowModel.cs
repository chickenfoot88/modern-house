using System.Collections.Generic;
using System.Web.Mvc;

namespace Tbo.WebHost.Models.Registries.Waybills
{
    /// <summary>
    /// Модель для открытия окна создания/редактирвоания заявки
    /// </summary>
    /// <typeparam name="T">тип dto</typeparam>
    public class WaybillsEditWindowModel<T> where T:class
    {
        /// <summary>
        /// Данные
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Сторы для DropDownList-ов
        /// </summary>
        public WaybillsEditWindowStoresModel Stores { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">dto</param>
        /// <param name="cars">автомобили</param>
        public WaybillsEditWindowModel(T data, List<SelectListItem> cars)
        {
            Data = data;
            Stores = new WaybillsEditWindowStoresModel
            {
                Cars = cars
            };
        }
    }

    /// <summary>
    /// Модель сторов для DropDownList-ов
    /// </summary>
    public class WaybillsEditWindowStoresModel
    {
        /// <summary>
        /// Автомобили
        /// </summary>
        public List<SelectListItem> Cars { get; set; }
    }
}