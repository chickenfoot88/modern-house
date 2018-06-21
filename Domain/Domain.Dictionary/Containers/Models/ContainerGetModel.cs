using System;
using System.Linq.Expressions;
using Domain.Dictionary.Containers.Entities;
using Domain.Dictionary.Containers.Enums;
using Domain.Core.Positions.Models;
using Domain.Dictionary.ContainerTypes.Models;

namespace Domain.Dictionary.Containers.Models
{
    public class ContainerGetModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Номер контейнера
        /// </summary>
        public long Number { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Описание контейнера
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Статус контейнера
        /// </summary>
        public ContainerStatus Status { get; set; }

        /// <summary>
        /// Тип контейнера
        /// </summary>
        public ContainerTypeGetModel ContainerType { get; set; }
        public string ContainerTypeId => ContainerType?.Id.ToString();

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLatitude { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLongitude { get; set; }

        public ContainerGetModel()
        {
        }

        public ContainerGetModel(Container container)
        {
            this.Id = container.Id;
            this.Number = container.Number;
            this.Address = container.Address;
            this.Description = container.Description;
            this.Status = container.Status;
            this.ContainerType = container.ContainerType != null
                ? new ContainerTypeGetModel(container.ContainerType)
                : null;

            this.PositionLatitude = container.Position?.Latitude;
            this.PositionLongitude = container.Position?.Longitude;
        }

        public static Expression<Func<Container, ContainerGetModel>> ProjectionExpression =
            x => new ContainerGetModel
            {
                Id = x.Id,
                Number = x.Number,
                Address = x.Address,
                Description = x.Description,
                Status = x.Status,
                ContainerType = x.ContainerType == null
                    ? null
                    : new ContainerTypeGetModel
                    {
                        Id = x.ContainerType.Id,
                        Name = x.ContainerType.Name,
                        Capacity = x.ContainerType.Capacity.ToString()
                    }
            };
    }
}
