using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Domain.Dictionary.Cars.Entities;
using Domain.Core.Positions.Models;

namespace Domain.Dictionary.Cars.Models
{
    /// <summary>
    /// Модель для автомобиля
    /// </summary>
    public class CarGetModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Номер автомобиля
        /// </summary>
        [Display(Name = "Номер автомобиля")]
        public string Number { get; set; }

        /// <summary>
        /// Марка автомобиля автомобиля
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

        public CarGetModel()
        {

        }

        public CarGetModel(Car car)
        {
            this.Id = car.Id;
            this.Number = car.Number;
            this.Mark = car.Mark;
            this.Serviceability = car.Serviceability.ToString();
            this.PositionLatitude = car.Position?.Latitude;
            this.PositionLongitude = car.Position?.Longitude;
        }

        public static Expression<Func<Car, CarGetModel>> ProjectionExpression =
            x => new CarGetModel
            {
                Id = x.Id,
                Number = x.Number,
                Mark = x.Mark,
                Serviceability = x.Serviceability.ToString()
            };
    }
}
