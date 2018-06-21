using System;
using System.Linq;
using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Core.DataAccess.Params;
using Domain.Core.Positions.Interfaces;
using Domain.Dictionary.Cars.Entities;
using Domain.Dictionary.Containers.Entities;
using Domain.Dictionary.Customers.Entities;
using Domain.Dictionary.Customers.Interfaces;
using Domain.Dictionary.Customers.Models;
using Domain.Dictionary.Drivers.Entities;
using Domain.Dictionary.Polygons.Entities;
using Domain.Registries.Requests.Entities;
using Domain.Registries.Requests.Enums;

namespace Domain.Registries.Requests.Models
{
    public class RequestSaveModel
    {
        /// <summary>
        /// Дата создания заявки
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// Дата выполнения заявки
        /// </summary>
        public string ExecutionDate { get; set; }

        /// <summary>
        /// Дата заявки планируемая
        /// </summary>
        public string PlannedDate { get; set; }

        /// <summary>
        /// Дата забора планируемая
        /// </summary>
        public string PlannedUninstallDate { get; set; }

        /// <summary>
        /// Заказчик  - телефон
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Заказчик  - телефон
        /// </summary>
        public string CustomerPhone { get; set; }

        /// <summary>
        /// Заказчик - Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Заказчик - Контактное лицо
        /// </summary>
        public virtual string ContactPersonName { get; set; }

        /// <summary>
        /// Заказчик - Номер телефона контактного лица
        /// </summary>
        public virtual string ContactPersonPhone { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public string ContainerId { get; set; }

        /// <summary>
        /// Полигон
        /// </summary>
        public string PolygonId { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Автомобиль
        /// </summary>
        public string CarId { get; set; }

        /// <summary>
        /// Водитель
        /// </summary>
        public string DriverId { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Опалачено/ не оплачено
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// Статус заявки
        /// </summary>
        public RequestStatus Status { get; set; }

        /// <summary>
        /// Тип заявки
        /// </summary>
        public RequestType Type { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLatitude { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLongitude { get; set; }

        public void ApplyToEntity(Request request, IDataStore dataStore, IPositionService positionService,
            ICustomerService customerService)
        {
            if (request.Id == 0)
            {
                request.CreateDateTime = DateTime.Now;
            }

            request.PlannedDateTime = DateTime.Parse(this.PlannedDate);

            if (!string.IsNullOrEmpty(ExecutionDate))
                request.ExecutionDateTime = DateTime.Parse(this.ExecutionDate);

            request.Container = dataStore.FindById<Container>(this.ContainerId);
            request.Type = this.Type;

            //"малые" контейнеры - забор через 3 дня, "большие" - через день
            var daysCountForUninstall = request.Container?.ContainerType.Capacity <= 8 ? 3 : 1;

            request.PlannedUninstallDateTime = string.IsNullOrEmpty(PlannedUninstallDate)
                ? (request.Type == RequestType.Install ? request.PlannedDateTime.AddDays(daysCountForUninstall) : (DateTime?)null)
                : DateTime.Parse(this.PlannedUninstallDate);

            request.Address = this.Address;
            request.ContactPersonName = this.ContactPersonName;
            request.ContactPersonPhone = this.ContactPersonPhone;
            request.Comment = this.Comment;
            request.Status = this.Status;
            request.PaymentType = this.PaymentType;
            request.IsPaid = this.IsPaid ? Enums.IsPaid.Yes : Enums.IsPaid.No;
            request.Sum = this.Sum;

            request.Polygon = dataStore.FindById<Polygon>(this.PolygonId);
            request.Customer = dataStore.FindById<Customer>(this.CustomerId);
            request.Car = dataStore.FindById<Car>(this.CarId);
            request.Driver = dataStore.FindById<Driver>(this.DriverId);

            if (request.PositionId.HasValue)
            {
                positionService.Update(request.PositionId.Value, PositionLatitude, PositionLongitude);
            }
            else
            {
                request.Position = positionService.Create(PositionLatitude, PositionLongitude);
            }

            //создаем нового
            if (this.CustomerId == "0")
            {
                var customerDbEntity = customerService.GetAllCustomerModels(null)
                    .FirstOrDefault(x => x.Phone == CustomerPhone);

                if (customerDbEntity == null)
                {
                    var createModel = new CustomerSaveModel
                    {
                        Address = this.Address,
                        ContactPersonPhone = this.ContactPersonPhone,
                        ContactPersonName = this.ContactPersonName,
                        Name = this.CustomerName,
                        Phone = this.CustomerPhone,
                        PositionLatitude = request.Position?.Latitude,
                        PositionLongitude = request.Position?.Longitude
                    };
                    var customer = customerService.Create(createModel);

                    request.CustomerId = customer.Id;
                }
                else
                {
                    request.CustomerId = customerDbEntity.Id;
                }
            }

            request.Address = Address;
            request.ContactPersonName = ContactPersonName;
            request.ContactPersonPhone = ContactPersonPhone;
        }

        public static RequestSaveModel FromEntity(Request request)
        {
            var model = new RequestSaveModel();

            model.CustomerName = request.Customer?.Name;
            model.CustomerPhone = request.Customer?.Phone;
            model.Address = request.Address;
            model.ContactPersonName = request.ContactPersonName;
            model.ContactPersonPhone = request.ContactPersonPhone;
            model.Comment = request.Comment;
            model.Status = request.Status;
            model.Type = request.Type;
            model.PaymentType = request.PaymentType;
            model.IsPaid = request.IsPaid == Enums.IsPaid.Yes;
            model.Sum = request.Sum;
            model.PlannedDate = request.PlannedDateTime.ToString("dd.MM.yyyy");
            model.PlannedUninstallDate = request.PlannedUninstallDateTime?.ToString("dd.MM.yyyy");
            model.CreateDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            model.ContainerId = request.Container?.Id.ToString();
            model.PolygonId = request.Polygon?.Id.ToString();
            model.CustomerId = request.Customer?.Id.ToString();
            model.CarId = request.Car?.Id.ToString();
            model.DriverId = request.Driver?.Id.ToString();

            model.PositionLatitude = request.Position?.Latitude;
            model.PositionLongitude = request.Position?.Longitude;

            return model;
        }
    }
}
