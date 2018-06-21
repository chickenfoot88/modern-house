using System.Threading.Tasks;
using Domain.MobileApi.Models;

namespace Domain.MobileApi.Interfaces
{
    /// <summary>
    /// Сервис получения информации о пользователе-водителе
    /// </summary>
    public interface IDriverUserService
    {
        /// <summary>
        /// Получить информацию о пользователе-водителе
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        /// <returns></returns>
        DriverUserInfo GetDriverUserInfo(long userId);

        /// <summary>
        /// Обновить информацию о позиции автомобиля
        ///     водителя получаем по идентификатору пользователя
        ///     автомобиль находим по графику работ (на текущий день и по водителю)
        ///     обновляем координаты найденного автомобиля
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="latitude">широта</param>
        /// <param name="longitude">долгота</param>
        /// <returns></returns>
        Task UpdateCarPosition(long userId, decimal latitude, decimal longitude);
    }
}
