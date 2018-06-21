using Domain.Registries.Requests.Entities;
using System;
using System.Linq.Expressions;
using Domain.Registries.Requests.Enums;
using Domain.Dictionary.Cars.Models;
using Domain.Dictionary.Drivers.Models;
using Core.Extensions;

namespace Domain.Registries.Requests.Models
{
    public class RequestGetModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дата-время создания заявки
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        public string CreateDate => CreateDateTime.ToString("dd.MM.yyyy HH:mm");

        /// <summary>
        /// Дата-время выполнения заявки
        /// </summary>
        public DateTime? ExecutionDateTime { get; set; }
        public string ExecutionDate => ExecutionDateTime?.ToString("dd.MM.yyyy HH:mm");

        /// <summary>
        /// Дата-время планируемая
        /// </summary>
        public DateTime PlannedDateTime { get; set; }
        public string PlannedDate => PlannedDateTime.ToString("dd.MM.yyyy HH:mm");
        
        /// <summary>
        /// Дата забора планируемая
        /// </summary>
        public DateTime? PlannedUninstallDateTime { get; set; }
        public string PlannedUninstallDate => PlannedUninstallDateTime?.ToString("dd.MM.yyyy HH:mm");

        /// <summary>
        /// Заказчик - наименование
        /// </summary>
        public string CustomerName => Customer?.Name;

        /// <summary>
        /// Заказчик - телефон
        /// </summary>
        public string CustomerPhone => Customer?.Phone;

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Контактное лицо
        /// </summary>
        public string ContactPersonName { get; set; }

        /// <summary>
        /// Номер телефона контактного лица
        /// </summary>
        public string ContactPersonPhone { get; set; }

        /// <summary>
        /// Оплачена ли заявка
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Статус заявки
        /// </summary>
        public RequestStatus Status { get; set; }
        public string StatusName => Status.GetDescription();

        /// <summary>
        /// Тип заявки
        /// </summary>
        public RequestType Type { get; set; }
        public string TypeName => Type.GetDescription();

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public RequestContanerModel Container { get; set; }
        public string ContainerId => Container?.Id.ToString();

        /// <summary>
        /// Полигон
        /// </summary>
        public RequestPolygonModel Polygon { get; set; }
        public string PolygonId => Polygon?.Id.ToString();

        /// <summary>
        /// Заказчик
        /// </summary>
        public RequestCustomerModel Customer { get; set; }
        public string CustomerId => Customer?.Id.ToString();

        /// <summary>
        /// Автомобиль
        /// </summary>
        public CarGetModel Car { get; set; }
        public string CarId => Car?.Id.ToString();

        /// <summary>
        /// Водитель
        /// </summary>
        public DriverGetModel Driver { get; set; }
        public string DriverId => Driver?.Id.ToString();

        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLatitude { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLongitude { get; set; }

        public RequestGetModel()
        {
        }

        public RequestGetModel(Request request)
        {
            this.Id = request.Id;
            this.CreateDateTime = request.CreateDateTime;
            this.ExecutionDateTime = request.ExecutionDateTime;
            this.PlannedDateTime = request.PlannedDateTime;
            this.PlannedUninstallDateTime = request.PlannedUninstallDateTime;
            this.Address = request.Address;
            this.ContactPersonName = request.ContactPersonName;
            this.ContactPersonPhone = request.ContactPersonPhone;
            this.Comment = request.Comment;
            this.Status = request.Status;
            this.Type = request.Type;
            this.PaymentType = request.PaymentType;
            this.IsPaid = request.IsPaid == Enums.IsPaid.Yes;
            this.Sum = request.Sum;

            this.Container = request.Container != null
                ? new RequestContanerModel
                {
                    Id = request.Container.Id,
                    TypeId = request.Container.ContainerType?.Id ?? 0,
                    Type = request.Container.ContainerType?.Name
                }
                : null;

            this.Polygon = request.Polygon != null
                ? new RequestPolygonModel
                {
                    Id = request.Polygon.Id,
                    Name = request.Polygon.Name
                }
                : null;

            this.Customer = request.Customer != null
                ? new RequestCustomerModel
                {
                    Id = request.Customer.Id,
                    Name = request.Customer.Name,
                    Phone = request.Customer.Phone
                }
                : null;

            this.Car = request.Car != null
                ? new CarGetModel
                {
                    Id = request.Car.Id,
                    Mark = request.Car.Mark,
                    Number = request.Car.Number
                }
                : null;

            this.Driver = request.Driver != null
                ? new DriverGetModel
                {
                    Id = request.Driver.Id,
                    Name = request.Driver.Name,
                    FirstName = request.Driver.FirstName,
                    LastName = request.Driver.LastName,
                    Patronymic = request.Driver.Patronymic,
                    PhoneNumber = request.Driver.PhoneNumber
                }
                : null;


            this.PositionLatitude = request.Position?.Latitude;
            this.PositionLongitude = request.Position?.Longitude;
        }

        public static Expression<Func<Request, RequestGetModel>> ProjectionExpression =
            x => new RequestGetModel
            {
                Id = x.Id,
                ContactPersonName = x.ContactPersonName,
                ContactPersonPhone = x.ContactPersonPhone,
                CreateDateTime = x.CreateDateTime,
                ExecutionDateTime = x.ExecutionDateTime,
                PlannedDateTime = x.PlannedDateTime,
                PlannedUninstallDateTime = x.PlannedUninstallDateTime,
                Address = x.Address,
                Comment = x.Comment,
                Status = x.Status,
                Type = x.Type,
                PaymentType = x.PaymentType,
                IsPaid = x.IsPaid == Enums.IsPaid.Yes,
                Sum = x.Sum,

                Container = x.Container != null
                ? new RequestContanerModel
                {
                    Id = x.Container.Id,
                    TypeId = x.Container.ContainerType.Id,
                    Type = x.Container.ContainerType.Name
                }
                : null,

                Polygon = x.Polygon != null
                ? new RequestPolygonModel
                {
                    Id = x.Polygon.Id,
                    Name = x.Polygon.Name
                }
                : null,

                Customer = x.Customer != null
                ? new RequestCustomerModel
                {
                    Id = x.Customer.Id,
                    Name = x.Customer.Name,
                    Phone = x.Customer.Phone
                }
                : null,

                Car = x.Car != null
                ? new CarGetModel
                {
                    Id = x.Car.Id,
                    Mark = x.Car.Mark,
                    Number = x.Car.Number
                }
                : null,

                Driver = x.Driver != null
                ? new DriverGetModel
                {
                    Id = x.Driver.Id,
                    Name = x.Driver.Name,
                    FirstName = x.Driver.FirstName,
                    LastName = x.Driver.LastName,
                    Patronymic = x.Driver.Patronymic,
                    PhoneNumber = x.Driver.PhoneNumber
                }
                : null
            };
    }

    public class RequestContanerModel
    {
        public long Id { get; set; }

        public long TypeId { get; set; }

        public string Type { get; set; }
    }

    public class RequestPolygonModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class RequestCustomerModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
