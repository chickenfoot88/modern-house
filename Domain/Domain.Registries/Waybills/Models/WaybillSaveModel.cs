using System;
using System.Text.RegularExpressions;
using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Domain.Dictionary.Cars.Entities;
using Domain.Registries.Waybills.Entities;

namespace Domain.Registries.Waybills.Models
{
    public class WaybillSaveModel
    {
        /// <summary>
        /// Дата заявки
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Машина
        /// </summary>
        public string CarId { get; set; }

        public void ApplyToEntity(Waybill waybill, IDataStore dataStore)
        {
            waybill.WaybillDate = ParseDateTime(this.Date) ?? DateTime.Today;

            waybill.Car = dataStore.FindById<Car>(this.CarId);
        }

        private DateTime? ParseDateTime(string date)
        {
            var pattern = @"(?<day>\d{1,2})\.(?<month>\d{1,2})\.(?<year>\d{2,4})";

            var match = Regex.Match(date, pattern);
            if (match.Success)
            {
                return new DateTime(
                    int.Parse(match.Groups["year"].Value),
                    int.Parse(match.Groups["month"].Value),
                    int.Parse(match.Groups["day"].Value)
                );
            }

            return null;
        }
    }
}
