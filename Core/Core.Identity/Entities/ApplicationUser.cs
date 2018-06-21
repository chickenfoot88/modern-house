using System;
using Core.DataAccess;
using Microsoft.AspNet.Identity;

namespace Core.Identity.Entities
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class ApplicationUser : PersistentObject, IUser<long>
    {
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public virtual string Login { get; set; }

        /// <summary>
        /// Идентификатор файла, являющийся аватаром
        /// </summary>
        public virtual long? AvatarId { get; set; }

        /// <summary>
        /// Дата окончания блокировки
        /// </summary>
        public virtual DateTimeOffset? LockoutEndDate { get; set; }

        /// <summary>
        /// Признак блокировки
        /// </summary>
        public virtual bool Locked { get; set; }

        /// <summary>
        /// Число неудачных попыток входа
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        public virtual string PasswordHash { get; set; }
    }
}
