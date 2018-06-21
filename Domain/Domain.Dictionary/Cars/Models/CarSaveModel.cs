using System;
using System.Linq;
using Core.DataAccess.Interfaces;
using Domain.Core.Positions.Interfaces;
using Domain.Dictionary.Cars.Entities;
using Domain.Dictionary.Cars.Enums;

namespace Domain.Dictionary.Cars.Models
{
    /// <summary>
    /// Модель для автомобиля
    /// </summary>
    public class CarSaveModel
    {
        /// <summary>
        /// Номер автомобиля
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Марка автомобиля
        /// </summary>
        public string Mark { get; set; }

        /// <summary>
        /// Исправность
        /// </summary>
        public string Serviceability { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLatitude { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLongitude { get; set; }

        public void ApplyToEntity(Car car, IDataStore dataStore, IPositionService positionService)
        {
            car.Number = this.Number;
            car.Mark = this.Mark;

            car.Serviceability = Enum
                .GetValues(typeof(CarServiceability))
                .Cast<CarServiceability>()
                .FirstOrDefault(x => x.ToString() == this.Serviceability);

            if (car.PositionId.HasValue)
            {
                positionService.Update(car.PositionId.Value, PositionLatitude, PositionLongitude);
            }
            else
            {
                car.Position = positionService.Create(PositionLatitude, PositionLongitude);
            }
        }
    }
}
