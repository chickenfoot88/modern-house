using System;
using System.Linq.Expressions;
using Domain.Dictionary.Cars.Entities;

namespace Domain.Dictionary.Cars.Models
{
    public class CarContainerTypeGetModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Number { get; set; }

        /// <summary>
        /// Наименование типа контейнера
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Емкость контейнера
        /// </summary>
        public string Capacity { get; set; }



        public static Expression<Func<CarContainerType, CarContainerTypeGetModel>> ProjectionExpression =
            x => new CarContainerTypeGetModel
            {
                Id = x.Id,
                Capacity = x.ContainerType.Capacity.ToString(),
                Name = x.ContainerType.Name
            };
    }
}
