using Core.DataAccess;
using Core.Identity.Entities;

namespace Domain.Dictionary.Drivers.Entities
{
    /// <summary>
    /// Сущность водителя
    /// </summary>
    public class Driver: PersistentObject
    {
        /// <summary>
        /// Имя водителя
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Фамилия водителя
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Отчество водителя
        /// </summary>
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// ФИО водителя
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// Номер ВУ
        /// </summary>
        public virtual string DriverLicenceNumber { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual long? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
