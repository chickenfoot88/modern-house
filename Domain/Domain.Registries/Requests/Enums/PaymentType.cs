using System.ComponentModel;

namespace Domain.Registries.Requests.Enums
{
    /// <summary>
    /// Тип оплаты
    /// </summary>
    public enum PaymentType
    {
        /// <summary>
        /// Наличные
        /// </summary>
        [Description("Наличные")]
        Cash,

        /// <summary>
        /// Безналичный расчет
        /// </summary>
        [Description("Безналичный расчет")]
        NonCash,

        /// <summary>
        /// Перевод на карту
        /// </summary>
        [Description("Перевод на карту")]
        Transfer
    }
}
