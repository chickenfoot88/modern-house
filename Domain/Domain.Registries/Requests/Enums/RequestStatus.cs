using System.ComponentModel;

namespace Domain.Registries.Requests.Enums
{
    /// <summary>
    /// Статус заявки
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// Новая
        /// </summary>
        [Description("Новая")]
        New,

        /// <summary>
        /// В работе
        /// </summary>
        [Description("В работе")]
        InWork,

        /// <summary>
        /// Выполнена
        /// </summary>
        [Description("Выполнена")]
        Done,

        /// <summary>
        /// Отклонена
        /// </summary>
        [Description("Отклонена")]
        Rejected
    }
}
