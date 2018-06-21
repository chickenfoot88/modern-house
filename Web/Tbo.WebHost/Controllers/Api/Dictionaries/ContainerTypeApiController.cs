using Core.DataAccess.Params;
using Domain.Dictionary.ContainerTypes.Interfaces;
using Domain.Dictionary.ContainerTypes.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Tbo.WebHost.Attributes;
using Tbo.WebHost.Controllers.Api.Common;

namespace Tbo.WebHost.Controllers.Api.Dictionaries
{
    /// <summary>
    /// Контоллер для операций с водителями
    /// </summary>
    [RoutePrefix("api/containertypes")]
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class ContainerTypeApiController : BaseApiController
    {
        private readonly IContainerTypeService containerTypeService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="containerTypeService">Сервис для работы с типами контейнера</param>
        public ContainerTypeApiController(IContainerTypeService containerTypeService)
        {
            this.containerTypeService = containerTypeService;
        }

        /// <summary>
        /// Получить список типов контейнеров
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ResponseType(typeof(List<ContainerTypeGetModel>))]
        public HttpResponseMessage List(StoreLoadParams storeLoadParams)
        {
            var result = containerTypeService.GetAllContainerTypeModels(storeLoadParams).ToList();
            return Success(result);
        }

        /// <summary>
        /// Получить тип контейнера по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(ContainerTypeGetModel))]
        public HttpResponseMessage Get(long id)
        {
            var result = containerTypeService.GetContainerTypeModel(id);

            if (result == null)
            {
                return Failure("Тип контейнера с указанным идентификатором не найден");
            }

            return Success(result);
        }

        /// <summary>
        /// Добавление типа контейнера
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public async Task<HttpResponseMessage> Post(ContainerTypeSaveModel model)
        {
            if (model != null)
            {
                await containerTypeService.CreateAsync(model);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение типа контейнера
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Put(long id, ContainerTypeSaveModel model)
        {
            if (model != null)
            {
                await containerTypeService.UpdateAsync(id, model);
                return Success();
            }

            return Failure("Невозможно изменить пустой элемент");
        }

        /// <summary>
        /// Удаление типа контейнера
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Delete(long id)
        {
            await containerTypeService.DeleteAsync(id);
            return Success();
        }
    }
}