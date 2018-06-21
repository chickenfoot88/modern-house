using System.ComponentModel;

namespace Domain.Dictionary.Containers.Enums
{
    /// <summary>
    /// Статсу контейнера
    /// </summary>
    public enum ContainerStatus
    {
        /// <summary>
        /// Установлен
        /// </summary>
        [Description("Установлен")]
        Installed,

        /// <summary>
        /// Свободен
        /// </summary>
        [Description("Свободен")]
        Free
    }
}
