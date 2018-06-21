using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Core.DataAccess.Params;
using Domain.Dictionary.Cars.Interfaces;
using Domain.Dictionary.Cars.Models;
using Tbo.WebHost.Attributes;
using Tbo.WebHost.Controllers.Api.Common;
using System.Threading.Tasks;
using Domain.Core.Positions.Models;

namespace Tbo.WebHost.Controllers.Api.Dictionaries
{
    /// <summary>
    /// Контоллер для операций с автомобилями
    /// </summary>
    [RoutePrefix("api/cars")]
    [Authorize(Roles = "Manager,Director,Dispatcher", Users = "")]
    public class CarApiController : BaseApiController
    {
        private readonly ICarService carService;

        /// <summary>
        /// Конструктор с внедрением зависимости
        /// </summary>
        /// <param name="carService">Сервис для работы с автомобилями</param>
        public CarApiController(ICarService carService)
        {
            this.carService = carService;
        }

        /// <summary>
        /// Получить список автомобилей
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [ResponseType(typeof(List<CarGetModel>))]
        public HttpResponseMessage GetList(StoreLoadParams storeLoadParams = null)
        {
            var result = carService.GetAllCarModels(storeLoadParams);
            return Success(result);
        }

        /// <summary>
        /// Получить автомобиль по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [Route("{id}")]
        [ResponseType(typeof(CarGetModel))]
        public HttpResponseMessage Get(long id)
        {
            var result = carService.GetCarModel(id);

            if(result == null)
            {
                return Failure("Автомобиль с указанным идентификатором не найден");
            }

            return Success(result);
        }

        /// <summary>
        /// Добавление автомобиля
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public async Task<HttpResponseMessage> Create(CarSaveModel model)
        {
            if (model != null)
            {
                await carService.CreateAsync(model);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение автомобиля
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Put(long id, CarSaveModel model)
        {
            if (model != null)
            {
                await carService.UpdateAsync(id, model);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Изменение координат автомобиля
        /// </summary>
        /// <returns></returns>
        [Route("{id}/position")]
        [HttpPut]
        public async Task<HttpResponseMessage> PositionPut(long id, PositionModel model)
        {
            if (model != null)
            {
                await carService.UpdatePositionAsync(id, model);
                return Success();
            }

            return Failure("Невозможно добавить пустой элемент");
        }

        /// <summary>
        /// Удаление автомобиля
        /// </summary>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<HttpResponseMessage> Delete(long id)
        {
            await carService.DeleteAsync(id);
            return Success();
        }

        /// <summary>
        /// Добавление типа контейнера к автомобилю
        /// </summary>
        /// <returns></returns>
        [Route("{id}/container-type")]
        [HttpPost]
        public async Task<HttpResponseMessage> AddCarContainerType(long id, [FromBody]CarContainerTypeSaveModel saveModel)
        {
            await carService.AddCarContainerTypeAsync(id, saveModel.ContainerTypeId);
            return Success();
        }

        /// <summary>
        /// Удаление типа контейнера у автомобилю
        /// </summary>
        /// <returns></returns>
        [Route("container-type/{id}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteCarContainerType(long id)
        {
            await carService.DeleteCarContainerTypeAsync(id);
            return Success();
        }
    }
}
