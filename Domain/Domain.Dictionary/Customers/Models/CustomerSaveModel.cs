using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Domain.Core.Positions.Interfaces;
using Domain.Core.Positions.Models;
using Domain.Dictionary.Customers.Entities;

namespace Domain.Dictionary.Customers.Models
{
    public class CustomerSaveModel
    {
        /// <summary>
        /// Номер заказчика
        /// </summary>
        public long Number { get; set; }

        /// <summary>
        /// Инн заказчика
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// Статус заказчика
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Наименование заказчика
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер телефона заказчика
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Фамилия, имя контактного лица
        /// </summary>
        public string ContactPersonName { get; set; }

        /// <summary>
        /// Номер телефона контактного лица
        /// </summary>
        public string ContactPersonPhone { get; set; }

        /// <summary>
        /// Адрес заказчика
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Заблокирован
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Описание заказчика
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public PositionModel Position { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLatitude { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLongitude { get; set; }

        public void ApplyToEntity(Customer customer, IDataStore dataStore, IPositionService positionService)
        {
            customer.Number = this.Number;
            customer.Description = this.Description;
            customer.Status = this.Status;
            customer.Position = this.Position?.ToEntity();
            customer.Name = this.Name;
            customer.Phone = this.Phone;
            customer.ContactPersonName = this.ContactPersonName;
            customer.ContactPersonPhone = this.ContactPersonPhone;
            customer.Inn = this.Inn;
            customer.Address = this.Address;
            customer.IsBlocked = this.IsBlocked;

            if (customer.PositionId.HasValue)
            {
                positionService.Update(customer.PositionId.Value, PositionLatitude, PositionLongitude);
            }
            else
            {
                customer.Position = positionService.Create(PositionLatitude, PositionLongitude);
            }
        }
    }
}
