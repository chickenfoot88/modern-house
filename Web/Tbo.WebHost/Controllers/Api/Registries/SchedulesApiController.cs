using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Domain.Registries.Schedules.Interfaces;
using Domain.Registries.Schedules.Models;
using Tbo.WebHost.Controllers.Api.Common;

namespace Tbo.WebHost.Controllers.Api.Registries
{
    /// <summary>
    /// Контоллер для операций с графиком работ
    /// </summary>
    [RoutePrefix("api/schedules")]
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class SchedulesApiController : BaseApiController
    {
        private readonly ISchedulesService schedulesService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="schedulesService">Сервис для работы c графиком работ</param>
        public SchedulesApiController(ISchedulesService schedulesService)
        {
            this.schedulesService = schedulesService;
        }

        /// <summary>
        /// Получить график работ
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(List<ScheduleGetModel>))]
        public HttpResponseMessage GetList()
        {
            var result = schedulesService.GetAllModels();
            return Success(result);
        }

        /// <summary>
        /// Получить график работ
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(ScheduleGetModel))]
        public HttpResponseMessage Get(long id)
        {
            var result = schedulesService.GetModel(id);

            if (result == null)
            {
                return Failure("График работ с указанным идентификатором не найден");
            }

            return Success(result);
        }

        /// <summary>
        /// Добавление графика работ
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public async Task<HttpResponseMessage> Post(UnwrapModel<ScheduleSaveModel> model)
        {
            if (model?.Data != null)
            {
                model.Data.DateStr = HttpUtility.UrlDecode(model.Data.DateStr);

                await schedulesService.CreateAsync(model.Data);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение графика работ
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Put(long id, UnwrapModel<ScheduleSaveModel> model)
        {
            if (model?.Data != null)
            {
                model.Data.DateStr = HttpUtility.UrlDecode(model.Data.DateStr);

                await schedulesService.UpdateAsync(id, model.Data);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Удаление заявки
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Delete(long id)
        {
            await schedulesService.DeleteAsync(id);
            return Success();
        }
    }
}