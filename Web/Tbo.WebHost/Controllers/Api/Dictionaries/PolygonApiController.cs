using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Core.DataAccess.Params;
using Domain.Core.Positions.Models;
using Domain.Dictionary.Polygons.Interfaces;
using Domain.Dictionary.Polygons.Models;
using Tbo.WebHost.Attributes;
using Tbo.WebHost.Controllers.Api.Common;

namespace Tbo.WebHost.Controllers.Api.Dictionaries
{
    /// <summary>
    /// Контоллер для операций с полигонами
    /// </summary>
    [RoutePrefix("api/polygons")]
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class PolygonApiController : BaseApiController
    {
        private readonly IPolygonService polygonService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="polygonService">Сервис для работы с полигонами</param>
        public PolygonApiController(IPolygonService polygonService)
        {
            this.polygonService = polygonService;
        }

        /// <summary>
        /// Получить список полигонов
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(List<PolygonGetModel>))]
        [HttpGet]
        public HttpResponseMessage List(StoreLoadParams storeLoadParams = null)
        {
            var result = polygonService.GetAllPolygonModels(storeLoadParams);
            return Success(result);
        }

        /// <summary>
        /// Получить полигон по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(PolygonGetModel))]
        public HttpResponseMessage Get(long id)
        {
            var result = polygonService.GetPolygonModel(id);

            if (result == null)
            {
                return Failure("Полигон с указанным идентификатором не найден");
            }

            return Success(result);
        }

        /// <summary>
        /// Добавление полигона
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public async Task<HttpResponseMessage> Post(PolygonSaveModel model)
        {
            if (model != null)
            {
                await polygonService.CreateAsync(model);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение полигона
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Put(long id, PolygonSaveModel model)
        {
            if (model != null)
            {
                await polygonService.UpdateAsync(id, model);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение координат полигона
        /// </summary>
        /// <returns></returns>
        [Route("{id}/position")]
        [HttpPut]
        public async Task<HttpResponseMessage> PositionPut(long id, PositionModel model)
        {
            if (model != null)
            {
                await polygonService.UpdatePositionAsync(id, model);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Удаление полигона
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Delete(long id)
        {
            await polygonService.DeleteAsync(id);
            return Success();
        }
    }
}