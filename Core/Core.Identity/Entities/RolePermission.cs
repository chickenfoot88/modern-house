using System.ComponentModel;
using Core.DataAccess.Interfaces;
using Core.Enums;

namespace Core.Identity.Entities
{
    /// <summary>
    /// Разрешение роли
    /// </summary>
    [Description("Разрешение роли")]
    public class RolePermission : IEntity
    {
        /// <summary>
        /// Роль
        /// </summary>
        public virtual string RoleId { get; set; }

        /// <summary>
        /// Разрешение
        /// </summary>
        public virtual Permission Permission { get; set; }
    }
}
