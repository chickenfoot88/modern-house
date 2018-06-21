using Core.DataAccess;
using Domain.Core.Positions.Entities;
using Domain.Dictionary.Cars.Enums;

namespace Domain.Dictionary.Cars.Entities
{
    /// <summary>
    /// Сущность автомобиля
    /// </summary>
    public class Car : PersistentObject
    {
        /// <summary>
        /// Регистрационный номер автомобиля
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Марка автомобиля автомобиля
        /// </summary>
        public virtual string Mark { get; set; }

        /// <summary>
        /// Исправность автомобиля
        /// </summary>
        public CarServiceability Serviceability { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public virtual long? PositionId { get; set; }
        public virtual Position Position { get; set; }
    }
}
