using System.ComponentModel;

namespace Core.Enums
{
    /// <summary>
    /// Роль
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// Директор
        /// </summary>
        [Description("Директор")]
        Director = 10,

        /// <summary>
        /// Менеджер
        /// </summary>
        [Description("Менеджер")]
        Manager = 20,

        /// <summary>
        /// Водитель
        /// </summary>
        [Description("Водитель")]
        Driver = 30,

        /// <summary>
        /// Диспетчер
        /// </summary>
        [Description("Диспетчер")]
        Dispatcher = 40
    }
}
