using System;
using System.Linq.Expressions;
using Domain.Core.Positions.Models;
using Domain.Dictionary.Polygons.Entities;

namespace Domain.Dictionary.Polygons.Models
{
    public class PolygonGetModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Номер полигона
        /// </summary>
        public long Number { get; set; }

        /// <summary>
        /// Статус полигона
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Наименование полигона
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер телефона полигона
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Адрес полигона
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Описание полигона
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLatitude { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLongitude { get; set; }

        public PolygonGetModel()
        {

        }

        public PolygonGetModel(Polygon polygon)
        {
            this.Id = polygon.Id;
            this.Name = polygon.Name;
            this.Number = polygon.Number;
            this.Description = polygon.Description;
            this.Status = polygon.Status;
            this.Address = polygon.Address;
            this.Phone = polygon.Phone;

            this.PositionLatitude = polygon.Position?.Latitude;
            this.PositionLongitude = polygon.Position?.Longitude;
        }

        public static Expression<Func<Polygon, PolygonGetModel>> ProjectionExpression =
            x => new PolygonGetModel
            {
                Id = x.Id,
                Number = x.Number,
                Name = x.Name,
                Description = x.Description,
                Status = x.Status,
                Address = x.Address,
                Phone = x.Phone
            };
    }
}
