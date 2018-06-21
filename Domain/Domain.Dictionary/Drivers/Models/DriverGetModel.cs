using Domain.Dictionary.Drivers.Entities;
using System;
using System.Linq.Expressions;

namespace Domain.Dictionary.Drivers.Models
{
    /// <summary>
    /// Модель для водителя
    /// </summary>
    public class DriverGetModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя водителя
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Номер ВУ
        /// </summary>
        public string DriverLicenceNumber { get; set; }

        /// <summary>
        /// Имя водителя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия водителя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество водителя
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public bool ChangePassword { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string UserPassword { get; set; }



        public DriverGetModel()
        {
        }

        public DriverGetModel(Driver driver)
        {
            Id = driver.Id;
            Name = driver.Name;
            DriverLicenceNumber = driver.DriverLicenceNumber;
            FirstName = driver.FirstName;
            LastName = driver.LastName;
            Patronymic = driver.Patronymic;
            PhoneNumber = driver.PhoneNumber;
            UserLogin = driver.User?.Login;
        }

        public static Expression<Func<Driver, DriverGetModel>> ProjectionExpression =
            x => new DriverGetModel
            {
                Id = x.Id,
                Name = x.Name,
                DriverLicenceNumber = x.DriverLicenceNumber,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Patronymic = x.Patronymic,
                PhoneNumber = x.PhoneNumber
            };
    }
}
