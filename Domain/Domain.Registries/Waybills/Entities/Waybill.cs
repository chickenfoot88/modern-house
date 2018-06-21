using System;
using Core.DataAccess;
using Domain.Dictionary.Cars.Entities;
using Domain.Registries.Requests.Entities;

namespace Domain.Registries.Waybills.Entities
{
    /// <summary>
    /// Сущность Маршрутного листа
    /// </summary>
    public class Waybill : PersistentObject
    {
        /// <summary>
        /// Дата маршрута
        /// </summary>
        public virtual DateTime WaybillDate { get; set; }

        /// <summary>
        /// Машина
        /// </summary>
        public virtual long CarId { get; set; }
        public virtual Car Car { get; set; }
    }
}
