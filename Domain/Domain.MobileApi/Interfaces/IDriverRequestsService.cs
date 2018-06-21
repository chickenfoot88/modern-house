using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.MobileApi.Models;

namespace Domain.MobileApi.Interfaces
{
    /// <summary>
    /// Сервис получения информации о заявках водителя
    /// </summary>
    public interface IDriverRequestsService
    {
        /// <summary>
        /// Список заявок на дату
        ///     водителя получаем по идентификатору пользователя
        ///     ищем водителя в заявка за выбранную дату
        /// </summary>
        /// <param name="userId">идентификатор пользователя</param>
        /// <param name="date">дата</param>
        /// <returns></returns>
        List<RequestListModel> GetList(long userId, DateTime date);

        /// <summary>
        /// Получение деталлизированной информации о заявке
        /// </summary>
        /// <param name="requestId">идентификатор заявки</param>
        /// <returns></returns>
        RequestGetModel GetById(long requestId);

        /// <summary>
        /// Завершить заявку
        /// </summary>
        /// <param name="requestId">идентификатор заявки</param>
        /// <returns></returns>
        Task CompleteRequest(long requestId);

        /// <summary>
        /// Отклонять заявку
        /// </summary>
        /// <param name="requestId">идентификатор заявки</param>
        /// <returns></returns>
        Task RejectRequest(long requestId);

        /// <summary>
        /// Взять в работу
        /// </summary>
        /// <param name="requestId">идентификатор заявки</param>
        /// <returns></returns>
        Task ProceedRequest(long requestId);
    }
}