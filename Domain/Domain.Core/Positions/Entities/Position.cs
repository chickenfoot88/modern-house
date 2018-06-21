using System;
using Core.DataAccess.Interfaces;

namespace Domain.Core.Positions.Entities
{
    /// <summary>
    /// Позиция - координаты местоположения
    /// </summary>
    public class Position : IEntity<long>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// Широта
        ///     принимает значения от -90 до +90
        /// </summary>
        private decimal _latitude;
        public virtual decimal Latitude
        {
            get { return _latitude; }
            set
            {
                BeforeSetPosition(value, _longitude);
                _latitude = value;
            }
        }


        /// <summary>
        /// Долгота
        ///     принимает значения от -90 до +90
        /// </summary>
        private decimal _longitude;
        public virtual decimal Longitude
        {
            get { return _longitude; }
            set
            {
                BeforeSetPosition(_latitude, value);
                _longitude = value;
            }
        }

        /// <summary>
        /// Проверка на не выход за границы допустимых значений
        /// </summary>
        /// <param name="latitude">широта</param>
        /// <param name="longitude">долгота</param>
        private void BeforeSetPosition(decimal latitude, decimal longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Широта может быть только в интервале от -90 до 90");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Долгота может быть только в интервале от -180 до 180");
            }
        }
    }
}
