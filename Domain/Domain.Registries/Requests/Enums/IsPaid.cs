using System.ComponentModel;

namespace Domain.Registries.Requests.Enums
{
    /// <summary>
    /// Оплачена ли заявка
    /// </summary>
    public enum IsPaid
    {
        [Description("Оплачено")]
        Yes = 1,

        [Description("Не оплачено")]
        No = 0
    }
}
