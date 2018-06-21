using System.Collections.Generic;
using System.Web.Mvc;

namespace Tbo.WebHost.Models.Registries.Schedules
{
    /// <summary>
    /// Модель для открытия окна создания/редактирвоания заявки
    /// </summary>
    /// <typeparam name="T">тип dto</typeparam>
    public class SchedulesEditWindowModel<T> where T:class
    {
        /// <summary>
        /// Данные
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Сторы для DropDownList-ов
        /// </summary>
        public SchedulesEditWindowStoresModel Stores { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">dto</param>
        public SchedulesEditWindowModel(T data)
        {
            Data = data;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public SchedulesEditWindowModel()
        {
        }
    }

    /// <summary>
    /// Модель сторов для DropDownList-ов
    /// </summary>
    public class SchedulesEditWindowStoresModel
    {

        /// <summary>
        /// Автомобили
        /// </summary>
        public List<SelectListItem> Cars { get; set; }

        /// <summary>
        /// Водители
        /// </summary>
        public List<SelectListItem> Drivers { get; set; }
    }
}