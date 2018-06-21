using Core.Extensions;
using Domain.Dictionary.ContainerTypes.Entities;

namespace Domain.Dictionary.ContainerTypes.Models
{
    public class ContainerTypeSaveModel
    {
        /// <summary>
        /// Наименование типа контейнера
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Емкость контейнера
        /// </summary>
        public string Capacity { get; set; }

        public void ApplyToEntity(ContainerType containerType)
        {
            containerType.Name = this.Name;
            containerType.Capacity = this.Capacity.ToDecimal();
        }
    }
}
