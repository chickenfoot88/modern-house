using System;
using Core.DataAccess;
using Domain.Dictionary.Cars.Entities;
using Domain.Dictionary.Drivers.Entities;

namespace Domain.Registries.Schedules.Entities
{
    /// <summary>
    /// График работы
    /// </summary>
    public class Schedule : PersistentObject
    {
        /// <summary>
        /// Дата создания
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Автомобиль
        /// </summary>
        public virtual long CarId { get; set; }
        public virtual Car Car { get; set; }

        /// <summary>
        /// Водитель
        /// </summary>
        public virtual long DriverId { get; set; }
        public virtual Driver Driver { get; set; }
    }
}
