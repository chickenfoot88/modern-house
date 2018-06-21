using Core.DataAccess;
using System;
using Domain.Core.Positions.Entities;
using Domain.Dictionary.Containers.Entities;
using Domain.Dictionary.Polygons.Entities;
using Domain.Dictionary.Customers.Entities;
using Domain.Registries.Requests.Enums;
using Domain.Dictionary.Cars.Entities;
using Domain.Dictionary.Drivers.Entities;

namespace Domain.Registries.Requests.Entities
{
    /// <summary>
    /// Сущность заявки
    /// </summary>
    public class Request: PersistentObject
    {
        /// <summary>
        /// Дата создания
        /// </summary>
        public virtual DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Дата выполнения
        /// </summary>
        public virtual DateTime? ExecutionDateTime { get; set; }

        /// <summary>
        /// Дата заявки планируемая
        /// </summary>
        public virtual DateTime PlannedDateTime { get; set; }

        /// <summary>
        /// Дата забора планируемая
        /// </summary>
        public virtual DateTime? PlannedUninstallDateTime { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Контактное лицо
        /// </summary>
        public virtual string ContactPersonName { get; set; }

        /// <summary>
        /// Номер телефона контактного лица
        /// </summary>
        public virtual string ContactPersonPhone { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public virtual Container Container { get; set; }
        public virtual long? ContainerId { get; set; }
        
        /// <summary>
        /// Полигон
        /// </summary>
        public virtual Polygon Polygon { get; set; }
        public virtual long? PolygonId { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public virtual Customer Customer { get; set; }
        public virtual long? CustomerId { get; set; }

        /// <summary>
        /// Автомобиль
        /// </summary>
        public virtual Car Car { get; set; }
        public virtual long? CarId { get; set; }

        /// <summary>
        /// Водитель
        /// </summary>
        public virtual Driver Driver { get; set; }
        public virtual long? DriverId { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Опалачено / не оплачено
        /// </summary>
        public virtual IsPaid IsPaid { get; set; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        public virtual PaymentType PaymentType { get;set;}

        /// <summary>
        /// Статус заявки
        /// </summary>
        public virtual RequestStatus Status { get; set; }

        /// <summary>
        /// Тип заявки
        /// </summary>
        public virtual RequestType Type { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public virtual Position Position { get; set; }
        public virtual long? PositionId { get; set; }
    }
}
