using Core.DataAccess;
using Domain.Registries.Requests.Entities;

namespace Domain.Registries.Waybills.Entities
{
    /// <summary>
    /// Сущность заявки Маршрутного листа
    /// </summary>
    public class WaybillRequest : PersistentObject
    {
        /// <summary>
        /// Порядковый номер
        /// </summary>
        public virtual int OrdinalNumber { get; set; }

        /// <summary>
        /// Маршрутный лист
        /// </summary>
        public virtual long WaybillId { get; set; }
        public virtual Waybill Waybill { get; set; }

        /// <summary>
        /// Заявка
        /// </summary>
        public virtual long RequestId { get; set; }
        public virtual Request Request { get; set; }
    }
}
