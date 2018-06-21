using System;
using System.Linq.Expressions;
using Domain.Dictionary.Customers.Entities;

namespace Domain.Dictionary.Customers.Models
{
    public class CustomerGetModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

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
        public decimal? PositionLatitude { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLongitude { get; set; }

        public CustomerGetModel()
        {

        }

        public CustomerGetModel(Customer customer)
        {
            this.Id = customer.Id;
            this.Number = customer.Number;
            this.Description = customer.Description;
            this.Inn = customer.Inn;
            this.Status = customer.Status;
            this.Address = customer.Address;
            this.Name = customer.Name;
            this.Phone = customer.Phone;
            this.ContactPersonName = customer.ContactPersonName;
            this.ContactPersonPhone = customer.ContactPersonPhone;
            this.IsBlocked = customer.IsBlocked;
            this.PositionLatitude = customer.Position?.Latitude;
            this.PositionLongitude = customer.Position?.Longitude;
        }

        public static Expression<Func<Customer, CustomerGetModel>> ProjectionExpression =
            x => new CustomerGetModel
            {
                Id = x.Id,
                Number = x.Number,
                Name = x.Name,
                Phone = x.Phone,
                ContactPersonName = x.ContactPersonName,
                ContactPersonPhone = x.ContactPersonPhone,
                Inn = x.Inn,
                Description = x.Description,
                Status = x.Status,
                Address = x.Address,
                IsBlocked = x.IsBlocked
            };
    }
}
