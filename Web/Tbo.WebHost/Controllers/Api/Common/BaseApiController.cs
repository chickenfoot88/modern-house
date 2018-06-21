using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Core.DataAccess.Params;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Tbo.WebHost.Models;

namespace Tbo.WebHost.Controllers.Api.Common
{
    /// <summary>
    /// Базовый контроллек Api
    /// </summary>
    [HostAuthentication(DefaultAuthenticationTypes.ApplicationCookie)]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Успешный ответ
        /// </summary>
        protected HttpResponseMessage Success()
        {
            var result = ResponseModel.Ok();

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Успешный ответ с данными
        /// </summary>
        /// <param name="obj">данные</param>
        protected HttpResponseMessage Success<T>(T obj)
        {
            var result = ResponseModel<T>.Ok(obj);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Успешный ответ с данными
        /// </summary>
        /// <param name="query"></param>
        /// <param name="loadParams"></param>
        protected HttpResponseMessage SuccessListFrom<T>(IQueryable<T> query, StoreLoadParams loadParams)
        {
            var result = ListResult<T>.From(query, loadParams);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Не успешный ответ с указанием ошибки и кода ответа
        /// </summary>
        /// <param name="error">ошибка</param>
        /// <param name="statusCode">код ответа</param>
        protected HttpResponseMessage Failure(string error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var result = ResponseModel.Failure(error);
            var response = Request.CreateResponse(HttpStatusCode.BadRequest);
            response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
            return response;
        }

        public class UnwrapModel<T> where T:class, new()
        {
            public T Data { get; set; }
        }
        
    }
}
