using System.Collections.Generic;
using System.Web.Mvc;
using Tbo.WebHost.Extensions;

namespace Tbo.WebHost.Models.Dictionaries.Containers
{
    /// <summary>
    /// Модель для открытия окна создания/редактирвоания контейнера
    /// </summary>
    /// <typeparam name="T">тип dto</typeparam>
    public class ContainerEditWindowModel<T> where T:class
    {
        /// <summary>
        /// Данные
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Сторы для DropDownList-ов
        /// </summary>
        public ContainerEditWindowStoresModel Stores { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="data">dto</param>
        /// <param name="containerStatuses">статусы контейнера</param>
        /// <param name="containerTypes">Типы контейнера</param>
        public ContainerEditWindowModel(T data, List<SelectListItem> containerStatuses, List<SelectListItem> containerTypes)
        {
            Data = data;
            Stores = new ContainerEditWindowStoresModel
            {
                ContainerStatuses = containerStatuses,
                ContainerTypes = containerTypes.AddEmptyElement()
            };
        }
    }

    /// <summary>
    /// Модель сторов для DropDownList-ов
    /// </summary>
    public class ContainerEditWindowStoresModel
    {
        /// <summary>
        /// Статусы контейнера
        /// </summary>
        public List<SelectListItem> ContainerStatuses { get; set; }

        /// <summary>
        /// Типы контейнера
        /// </summary>
        public List<SelectListItem> ContainerTypes { get; set; }
    }
}