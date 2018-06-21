using System.ComponentModel;

namespace Domain.Dictionary.Cars.Enums
{
    /// <summary>
    /// Исправность автомобиля
    /// </summary>
    public enum CarServiceability
    {
        /// <summary>
        /// Исправен
        /// </summary>
        [Description("Исправен")]
        Serviceable = 0,

        /// <summary>
        /// Не исправен
        /// </summary>
        [Description("Не исправен")]
        NotServiceable = 1
    }
}
