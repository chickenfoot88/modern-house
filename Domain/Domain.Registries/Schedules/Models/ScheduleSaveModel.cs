using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Domain.Dictionary.Cars.Entities;
using Domain.Dictionary.Drivers.Entities;
using Domain.Registries.Schedules.Entities;

namespace Domain.Registries.Schedules.Models
{
    public class ScheduleSaveModel
    {
        /// <summary>
        /// Дата
        /// </summary>
        public string DateStr { get; set; }

        /// <summary>
        /// Машина
        /// </summary>
        public string CarId { get; set; }


        /// <summary>
        /// Водитель
        /// </summary>
        public string DriverId { get; set; }

        public void ApplyToEntity(Schedule entity, IDataStore dataStore)
        {
            entity.Date = ParseDateTime(DateStr);
            entity.Car = dataStore.FindById<Car>(this.CarId);
            entity.Driver = dataStore.FindById<Driver>(this.DriverId);
        }

        private DateTime ParseDateTime(string dateTimeStr)
        {
            var pattern = @"(?<day>\d{1,2})\.(?<month>\d{1,2})\.(?<year>\d{2,4})";

            var match = Regex.Match(dateTimeStr, pattern);
            if (match.Success)
            {
                return new DateTime(
                    int.Parse(match.Groups["year"].Value),
                    int.Parse(match.Groups["month"].Value),
                    int.Parse(match.Groups["day"].Value)
                );
            }

            return DateTime.Now;
        }
    }
}
