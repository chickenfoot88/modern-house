using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Domain.Core.Positions.Interfaces;
using Domain.Core.Positions.Models;
using Domain.Dictionary.Containers.Entities;
using Domain.Dictionary.Containers.Enums;

namespace Domain.Dictionary.Containers.Models
{
    public class ContainerSaveModel
    {
        /// <summary>
        /// Номер контейнера
        /// </summary>
        public long Number { get; set; }

        /// <summary>
        /// Описание контейнера
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Статус контейнера
        /// </summary>
        public ContainerStatus Status { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public PositionModel Position { get; set; }

        /// <summary>
        /// Идентификатор типа контейнера
        /// </summary>
        public string ContainerTypeId { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLatitude { get; set; }

        /// <summary>
        /// Позиция - координаты местоположения
        /// </summary>
        public decimal? PositionLongitude { get; set; }

        public void ApplyToEntity(Container container, IDataStore dataStore, IPositionService positionService)
        {
            container.Number = this.Number;
            container.Description = this.Description;
            container.Status = this.Status;
            container.Position = this.Position?.ToEntity();
            container.Address = this.Address;
            container.ContainerType = dataStore.FindById<ContainerTypes.Entities.ContainerType>(this.ContainerTypeId);

            if (container.PositionId.HasValue)
            {
                positionService.Update(container.PositionId.Value, PositionLatitude, PositionLongitude);
            }
            else
            {
                container.Position = positionService.Create(PositionLatitude, PositionLongitude);
            }
        }
    }
}
