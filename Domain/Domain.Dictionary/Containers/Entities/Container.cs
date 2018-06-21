using Core.DataAccess;
using Domain.Core.Positions.Entities;
using Domain.Dictionary.Containers.Enums;
using Domain.Dictionary.ContainerTypes.Entities;

namespace Domain.Dictionary.Containers.Entities
{
    /// <summary>
    /// Сущность Контейнера
    /// </summary>
    public class Container: PersistentObject
    {
        /// <summary>
        /// Номер контейнера
        /// </summary>
        public virtual long Number { get; set; }

        /// <summary>
        /// Описание контейнера
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Статус контейнера
        /// </summary>
        public virtual ContainerStatus Status { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public virtual long? PositionId { get; set; }
        public virtual Position Position { get; set; }

        /// <summary>
        /// Тип контейнера
        /// </summary>
        public virtual long? ContainerTypeId { get; set; }
        public virtual ContainerType ContainerType { get; set; }
    }
}
