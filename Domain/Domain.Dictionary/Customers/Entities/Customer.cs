using Core.DataAccess;
using Domain.Core.Positions.Entities;

namespace Domain.Dictionary.Customers.Entities
{
    /// <summary>
    /// Сущность заказчика
    /// </summary>
    public class Customer : PersistentObject
    {
        /// <summary>
        /// Номер заказчика
        /// </summary>
        public virtual long Number { get; set; }

        /// <summary>
        /// Инн заказчика
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// Статус заказчика
        /// </summary>
        public virtual string Status { get; set; }

        /// <summary>
        /// Наименование заказчика
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер телефона заказчика
        /// </summary>
        public virtual string Phone { get; set; }
        
        /// <summary>
        /// Фамилия, имя контактного лица
        /// </summary>
        public virtual string ContactPersonName { get; set; }

        /// <summary>
        /// Номер телефона контактного лица
        /// </summary>
        public virtual string ContactPersonPhone { get; set; }

        /// <summary>
        /// Адрес заказчика
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Заблокирован
        /// </summary>
        public virtual bool IsBlocked { get; set; }

        /// <summary>
        /// Описание заказчика
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public virtual long? PositionId { get; set; }
        public virtual Position Position { get; set; }
    }
}
