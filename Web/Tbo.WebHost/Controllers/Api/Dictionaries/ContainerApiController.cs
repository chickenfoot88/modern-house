using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Core.DataAccess.Params;
using Domain.Dictionary.Containers.Interfaces;
using Domain.Dictionary.Containers.Models;
using Tbo.WebHost.Attributes;
using Tbo.WebHost.Controllers.Api.Common;
using System.Threading.Tasks;
using Domain.Core.Positions.Models;

namespace Tbo.WebHost.Controllers.Api.Dictionaries
{
    /// <summary>
    /// Контоллер для операций с контейнерами
    /// </summary>
    [RoutePrefix("api/containers")]
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class ContainerApiController : BaseApiController
    {
        private readonly IContainerService containerService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="containerService">Сервис для работы с контейнерами</param>
        public ContainerApiController(IContainerService containerService)
        {
            this.containerService = containerService;
        }

        /// <summary>
        /// Получить список контейнеров
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(List<ContainerGetModel>))]
        public HttpResponseMessage GetList([FromUri]StoreLoadParams storeLoadParams = null)
        {
            var result = containerService.GetAllContainerModels(storeLoadParams);
            return Success(result);
        }

        /// <summary>
        /// Получить контейнер по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(ContainerGetModel))]
        public HttpResponseMessage Get(long id)
        {
            var result = containerService.GetContainerModel(id);

            if (result == null)
            {
                return Failure("Контейнер с указанным идентификатором не найден");
            }

            return Success(result);
        }

        /// <summary>
        /// Добавление контейнера
        /// </summary>
        [Route("")]
        public async Task<HttpResponseMessage> Post(UnwrapModel<ContainerSaveModel> model)
        {
            if (model?.Data != null)
            {
                await containerService.CreateAsync(model.Data);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение контейнера
        /// </summary>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Put(long id, UnwrapModel<ContainerSaveModel> model)
        {
            if (model?.Data != null)
            {
                await containerService.UpdateAsync(id, model.Data);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение координат контейнера
        /// </summary>
        /// <returns></returns>
        [Route("{id}/position")]
        [HttpPut]
        public async Task<HttpResponseMessage> PositionPut(long id, PositionModel model)
        {
            if (model != null)
            {
                await containerService.UpdatePositionAsync(id, model);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Удаление контейнера
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Delete(long id)
        {
            await containerService.DeleteAsync(id);
            return Success();
        }
    }
}