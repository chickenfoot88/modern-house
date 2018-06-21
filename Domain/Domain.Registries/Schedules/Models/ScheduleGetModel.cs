using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Registries.Schedules.Entities;

namespace Domain.Registries.Schedules.Models
{
    public class ScheduleGetModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
        public string DateStr => Date.ToString("dd.MM.yyyy");

        /// <summary>
        /// Машина
        /// </summary>
        public ScheduleCarModel Car { get; set; }
        public string CarId => Car.Id.ToString();


        /// <summary>
        /// Водитель
        /// </summary>
        public ScheduleDriverModel Driver { get; set; }
        public string DriverId => Driver.Id.ToString();

        public ScheduleGetModel() { }
        public ScheduleGetModel(Schedule entity)
        {
            this.Id = entity.Id;
            this.Car = new ScheduleCarModel
            {
                Id = entity.CarId,
                Number = entity.Car.Number
            };
            this.Driver = new ScheduleDriverModel
            {
                Id = entity.DriverId,
                Name = entity.Driver.Name
            };
            this.Date = entity.Date;
        }

        public static Expression<Func<Schedule, ScheduleGetModel>> ProjectionExpression =
            x => new ScheduleGetModel
            {
                Id = x.Id,
                Car = new ScheduleCarModel
                {
                    Id = x.CarId,
                    Number = x.Car.Number
                },
                Driver = new ScheduleDriverModel
                {
                    Id = x.DriverId,
                    Name = x.Driver.Name
                },
                Date = x.Date
            };
    }

    public class ScheduleCarModel
    {
        public long Id { get; set; }

        public string Number { get; set; }
    }

    public class ScheduleDriverModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
