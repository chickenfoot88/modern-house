using Domain.Dictionary.ContainerTypes.Entities;
using System;
using System.Linq.Expressions;

namespace Domain.Dictionary.ContainerTypes.Models
{
    public class ContainerTypeGetModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование типа контейнера
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Емкость контейнера
        /// </summary>
        public string Capacity { get; set; }

        public ContainerTypeGetModel()
        {
        }

        public ContainerTypeGetModel(ContainerType containerType)
        {
            this.Id = containerType.Id;
            this.Name = containerType.Name;
            this.Capacity = containerType.Capacity.ToString();
        }

        public static Expression<Func<ContainerType, ContainerTypeGetModel>> ProjectionExpression =
            x => new ContainerTypeGetModel
            {
                Id = x.Id,
                Name = x.Name,
                Capacity = x.Capacity.ToString()
            };
    }
}
