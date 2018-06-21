using Core.DataAccess;
using Domain.Core.Positions.Entities;

namespace Domain.Dictionary.Polygons.Entities
{
    /// <summary>
    /// Сущность полигона
    /// </summary>
    public class Polygon: PersistentObject
    {
        /// <summary>
        /// Номер полигона
        /// </summary>
        public virtual long Number { get; set; }

        /// <summary>
        /// Статус полигона
        /// </summary>
        public virtual string Status { get; set; }

        /// <summary>
        /// Наименование полигона
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер телефона полигона
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// Адрес полигона
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Описание полигона
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public virtual long? PositionId { get; set; }
        public virtual Position Position { get; set; }
    }
}
