using Core.DataAccess;

namespace Domain.Dictionary.ContainerTypes.Entities
{
    /// <summary>
    /// Тип контейнера
    /// </summary>
    public class ContainerType : PersistentObject
    {
        /// <summary>
        /// Наименование типа контейнера
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Емкость контейнера
        /// </summary>
        public virtual decimal Capacity { get; set; }
    }
}
