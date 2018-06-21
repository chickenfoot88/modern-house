using Domain.Core.Positions.Interfaces;
using Domain.Core.Positions.Models;
using Domain.Dictionary.Polygons.Entities;

namespace Domain.Dictionary.Polygons.Models
{
    public class PolygonSaveModel
    {
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

        public void ApplyToEntity(Polygon polygon, IPositionService positionService)
        {
            polygon.Number = this.Number;
            polygon.Description = this.Description;
            polygon.Name = this.Name;
            polygon.Status = this.Status;
            polygon.Phone = this.Phone?.Replace("%2B", "+");
            polygon.Address = this.Address;

            if (polygon.PositionId.HasValue)
            {
                positionService.Update(polygon.PositionId.Value, PositionLatitude, PositionLongitude);
            }
            else
            {
                polygon.Position = positionService.Create(PositionLatitude, PositionLongitude);
            }
        }
    }
}
