using Core.DataAccess;
using Domain.Dictionary.ContainerTypes.Entities;

namespace Domain.Dictionary.Cars.Entities
{
    /// <summary>
    /// Тип контейнеров, которые может возить автомобиль
    /// </summary>
    public class CarContainerType : PersistentObject
    {
        public virtual long CarId { get; set; }
        public virtual Car Car { get; set; }

        public virtual long ContainerTypeId { get; set; }
        public virtual ContainerType ContainerType { get; set; }
    }
}
