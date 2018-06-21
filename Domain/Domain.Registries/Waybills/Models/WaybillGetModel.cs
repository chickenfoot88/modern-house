using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.DataAccess.Interfaces;
using Domain.Registries.Waybills.Entities;

namespace Domain.Registries.Waybills.Models
{
    public class WaybillGetModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дата маршрута
        /// </summary>
        public DateTime WaybillDateTime { get; set; }
        public string WaybillDate => WaybillDateTime.ToString("dd.MM.yyyy");

        /// <summary>
        /// Машина
        /// </summary>
        public WaybillCarModel Car { get; set; }
        public string CarId => Car?.Id.ToString();

        /// <summary>
        /// Машина
        /// </summary>
        public List<WaybillRequestGetModel> WaybillRequests { get; set; }

        public WaybillGetModel()
        {
        }

        public WaybillGetModel(Waybill waybill, IDataStore dataStore)
        {
            this.Id = waybill.Id;

            this.WaybillDateTime = waybill.WaybillDate;

            this.WaybillRequests = dataStore.GetAll<WaybillRequest>()
                .Where(x => x.WaybillId == waybill.Id)
                .Select(WaybillRequestGetModel.ProjectionExpression)
                .ToList();

            this.Car = waybill.Car != null
                ? new WaybillCarModel
                {
                    Id = waybill.Car.Id,
                    Name = waybill.Car.Mark + " " + waybill.Car.Number
                }
                : null;
        }

        public static Expression<Func<Waybill, WaybillGetModel>> ProjectionExpression =
            x => new WaybillGetModel
            {
                Id = x.Id,
                WaybillDateTime = x.WaybillDate,

                Car = x.Car != null
                ? new WaybillCarModel
                {
                    Id = x.Car.Id,
                    Name = x.Car.Mark + " " + x.Car.Number
                }
                : null
            };
    }

    public class WaybillCarModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
