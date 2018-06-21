using System;
using System.Linq.Expressions;
using Domain.Registries.Requests.Enums;
using Domain.Registries.Waybills.Entities;

namespace Domain.Registries.Waybills.Models
{
    public class WaybillRequestGetModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int OrdinalNumber { get; set; }

        /// <summary>
        /// Заявка
        /// </summary>
        public RequestLiteModel Request { get; set; }
        
        public WaybillRequestGetModel()
        {
        }

        public WaybillRequestGetModel(WaybillRequest waybillRequest)
        {
            this.Id = waybillRequest.Id;
            this.OrdinalNumber = waybillRequest.OrdinalNumber;

            this.Request = waybillRequest.Request != null
                ? new RequestLiteModel
                {
                    Id = waybillRequest.Request.Id,
                    Address = waybillRequest.Request.Address,
                    RequestStatus = waybillRequest.Request.Status
                }
                : null;
        }

        public static Expression<Func<WaybillRequest, WaybillRequestGetModel>> ProjectionExpression =
            x => new WaybillRequestGetModel
            {
                Id = x.Id,
                OrdinalNumber = x.OrdinalNumber,
                Request = x.Request != null
                    ? new RequestLiteModel
                    {
                        Id = x.Request.Id,
                        Address = x.Request.Address,
                        RequestStatus = x.Request.Status
                    }
                    : null
            };
    }

    public class RequestLiteModel
    {
        public long Id { get; set; }

        public string Address { get; set; }

        public RequestStatus RequestStatus { get; set; }
    }
}
