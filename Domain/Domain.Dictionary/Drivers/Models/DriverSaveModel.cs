using System.Linq;
using System.Threading.Tasks;
using Core.DataAccess.Extensions;
using Core.DataAccess.Interfaces;
using Core.Exceptions;
using Core.Identity.Entities;
using Core.Identity.Services;
using Domain.Dictionary.Drivers.Entities;

using Role = Core.Enums.Role;

namespace Domain.Dictionary.Drivers.Models
{
    /// <summary>
    /// Модель для водителя
    /// </summary>
    public class DriverSaveModel
    {
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
        /// Логин создаваемого пользователя
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Пароль создаваемого пользователя
        /// </summary>
        public bool ChangePassword { get; set; }

        /// <summary>
        /// Пароль создаваемого пользователя
        /// </summary>
        public string UserPassword { get; set; }

        public async Task ApplyToEntity(Driver driver, IDataStore dataStore, ApplicationUserManager userManager)
        {
            driver.Name = this.Name;
            driver.DriverLicenceNumber = this.DriverLicenceNumber;
            driver.FirstName = this.FirstName;
            driver.LastName = this.LastName;
            driver.Patronymic = this.Patronymic;
            driver.PhoneNumber = this.PhoneNumber?.Replace("%2B", "+");

            //считаем что User.Login неизменный с момента создания
            if (driver.User != null && this.ChangePassword)
            {
                var foundUser = await userManager.FindAsync(driver.User.Login, this.UserPassword);
                if (foundUser == null)
                {
                    await userManager.RemovePasswordAsync(driver.User.Id);
                    await userManager.AddPasswordAsync(driver.User.Id, this.UserPassword);
                }
            }
            else if (driver.User == null && !string.IsNullOrEmpty(this.UserLogin))
            {
                var user = new ApplicationUser
                {
                    Login = this.UserLogin,
                    UserName = this.Name
                };

                if (!string.IsNullOrEmpty(this.UserPassword))
                {
                    var createResult = await userManager.CreateAsync(user, this.UserPassword);
                    if (!createResult.Succeeded)
                    {
                        var errors = new[] {"Во время создания пользователя возникли ошибки."}
                            .Concat(createResult.Errors);
                        throw new ValidationException(string.Join("\r\n", errors));
                    }
                }
                else
                {
                    var createResult = await userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        var errors = new[] { "Во время создания пользователя возникли ошибки." }
                            .Concat(createResult.Errors);
                        throw new ValidationException(string.Join("\r\n", errors));
                    }
                }

                var addToRoleResult = await userManager.AddToRoleAsync(user.Id, Role.Driver.ToString());
                if (!addToRoleResult.Succeeded)
                {
                    var errors = new[] { "Во время привязки пользователя к роли возникли ошибки." }
                        .Concat(addToRoleResult.Errors);
                    throw new ValidationException(string.Join("\r\n", errors));
                }

                driver.UserId = user.Id;
            }
        }
    }
}
