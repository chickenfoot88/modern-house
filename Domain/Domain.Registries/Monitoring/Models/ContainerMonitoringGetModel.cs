using Domain.Dictionary.Containers.Entities;
using Domain.Dictionary.Containers.Enums;
using System;
using System.Linq.Expressions;

namespace Domain.Registries.Monitoring.Models
{
    public class ContainerMonitoringGetModel : MonitoringGetModel
    {
        /// <summary>
        /// Номер контейнера
        /// </summary>
        public virtual long Number { get; set; }

        /// <summary>
        /// Статус контейнера
        /// </summary>
        public virtual ContainerStatus Status { get; set; }

        /// <summary>
        /// Наименование типа контейнера
        /// </summary>
        public virtual string ContainerTypeName { get; set; }

        public static Expression<Func<Container, ContainerMonitoringGetModel>> ProjectionExpression =
            x => new ContainerMonitoringGetModel

            {
                Number = x.Number,
                ContainerTypeName = x.ContainerType != null ? x.ContainerType.Name : null,
                Status = x.Status,
                lat = x.Position.Latitude,
                lng = x.Position.Longitude
            };
    }
}

