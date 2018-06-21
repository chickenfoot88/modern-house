using System.ComponentModel;

namespace Domain.Registries.Requests.Enums
{
    /// <summary>
    /// Тип заявки
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// Постановка
        /// </summary>
        [Description("Постановка")]
        Install,

        /// <summary>
        /// Забор
        /// </summary>
        [Description("Забор")]
        Uninstall
    }
}
