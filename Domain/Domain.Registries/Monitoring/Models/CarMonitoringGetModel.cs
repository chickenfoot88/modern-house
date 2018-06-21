using Domain.Dictionary.Cars.Entities;
using System;
using System.Linq.Expressions;

namespace Domain.Registries.Monitoring.Models
{
    public class CarMonitoringGetModel : MonitoringGetModel
    {
        /// <summary>
        /// Регистрационный номер автомобиля
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Марка автомобиля автомобиля
        /// </summary>
        public virtual string Mark { get; set; }

        public static Expression<Func<Car, CarMonitoringGetModel>> ProjectionExpression =
            x => new CarMonitoringGetModel
            {
                Mark = x.Mark,
                Number = x.Number,
                lat = x.Position.Latitude,
                lng = x.Position.Longitude
            }; 
    }
}
