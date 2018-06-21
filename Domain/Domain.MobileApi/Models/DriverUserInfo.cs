namespace Domain.MobileApi.Models
{
    /// <summary>
    /// Информация о пользователе - водителе
    /// </summary>
    public class DriverUserInfo
    {
        /// <summary>
        /// Профиль пользователя
        /// </summary>
        public DriverUserProfile Profile { get; set; }

        /// <summary>
        /// Информация об автомобиле
        /// </summary>
        public DriverUserCar Car { get; set; }
    }

    /// <summary>
    /// Профиль пользователя
    /// </summary>
    public class DriverUserProfile
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string  LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronimyc { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }    
    }

    /// <summary>
    /// Информация об автомобиле водителя
    /// </summary>
    public class DriverUserCar
    {
        /// <summary>
        /// Марка автомобиля
        /// </summary>
        public string Mark { get; set; }

        /// <summary>
        /// Гос номер
        /// </summary>
        public string Number { get; set; }
    }
}
