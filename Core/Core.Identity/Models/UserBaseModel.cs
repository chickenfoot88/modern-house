using System;
using System.Linq.Expressions;
using Core.Identity.Entities;

namespace Core.Identity.Models
{
    /// <summary>
    /// Базовая модель пользователя
    /// </summary>
    public class UserBaseModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор файла, являющийся аватаром
        /// </summary>
        public virtual long? AvatarId { get; set; }

        /// <summary>
        /// Проекция сущности на модель
        /// </summary>
        public static Expression<Func<ApplicationUser, UserBaseModel>> ProjectionExpression =
            x => new UserBaseModel
            {
                Id = x.Id,
                Name = x.FullName,
                AvatarId = x.AvatarId
            };

    }
}
