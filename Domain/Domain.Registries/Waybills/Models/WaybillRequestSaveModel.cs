using System;
using System.Linq;
using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Core.Exceptions;
using Domain.Dictionary.Cars.Entities;
using Domain.Registries.Requests.Entities;
using Domain.Registries.Waybills.Entities;

namespace Domain.Registries.Waybills.Models
{
    public class WaybillRequestSaveModel
    {
        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int OrdinalNumber { get; set; }

        /// <summary>
        /// маршрутный лист
        /// </summary>
        public string WaybillId { get; set; }

        /// <summary>
        /// Заявка
        /// </summary>
        public string RequestId { get; set; }
        
        public void ApplyToEntity(WaybillRequest waybillRequest, IDataStore dataStore)
        {
            waybillRequest.OrdinalNumber = this.OrdinalNumber;

            var request = dataStore.FindById<Request>(this.RequestId);
            var waybill = dataStore.FindById<Waybill>(this.WaybillId);

            Validate(request, waybill, dataStore);

            waybillRequest.Request = request;
            waybillRequest.Waybill = waybill;
        }

        private void Validate(Request request, Waybill waybill, IDataStore dataStore)
        {
            var containerType = request?.Container?.ContainerType;
            if (containerType == null)
            {
                return;
            }

            var carContainerTypeExists = dataStore
                .GetAll<CarContainerType>()
                .Where(x => x.CarId == waybill.CarId)
                .Where(x => x.ContainerTypeId == containerType.Id)
                .Any();

            if (carContainerTypeExists)
            {
                return;
            }

            throw new ValidationException($"Данный автомобиль не может переводить данный тип контейнера ({request.Container.ContainerType.Name}, емкость: {request.Container.ContainerType.Capacity})");
        }
    }
}
