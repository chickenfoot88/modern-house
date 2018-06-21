using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tbo.WebHost.Models.Registries.Requests
{
    /// <summary>
    /// Модель страницы Index
    /// </summary>
    public class RequestsIndexModel
    {
        /// <summary>
        /// Варианты выбора для фильтров
        /// </summary>
        public RequestsIndexModelFilterStores FilterStores { get; set; }
    }

    /// <summary>
    /// Фильтры модели страницы Index
    /// </summary>
    public class RequestsIndexModelFilterStores
    {
        /// <summary>
        /// Типы контейнеров
        /// </summary>
        public List<SelectListItem> ContainerTypes { get; set; }

        /// <summary>
        /// Типы заявок
        /// </summary>
        public List<SelectListItem> RequestTypes { get; set; }

        /// <summary>
        /// Автомобили
        /// </summary>
        public List<SelectListItem> Cars { get; set; }

        /// <summary>
        /// Водители
        /// </summary>
        public List<SelectListItem> Drivers { get; set; }

        /// <summary>
        /// Статус заявки
        /// </summary>
        public List<SelectListItem> RequestStatuses { get; set; }

        /// <summary>
        /// Статус заявки
        /// </summary>
        public List<SelectListItem> IsPaids { get; set; }
    }
}