using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
using Core.Exceptions;
using Newtonsoft.Json;
using Tbo.WebHost.Models;

namespace Tbo.WebHost.Filters
{
    /// <summary>
    /// Фильтр для отлова исключений в Api
    /// </summary>
    public class WebApiExceptionFilter: ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exc = actionExecutedContext.Exception;
            if (exc is ValidationException)
            {
                var result = ResponseModel.Failure(exc.Message);
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.BadRequest);
                actionExecutedContext.Response.Content = new StringContent(JsonConvert.SerializeObject(result),
                    Encoding.UTF8, "application/json");
            }
            else
            {
                var result = ResponseModel.Failure(exc.Message);
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError);
                actionExecutedContext.Response.Content = new StringContent(JsonConvert.SerializeObject(result),
                    Encoding.UTF8, "application/json");
            }

            base.OnException(actionExecutedContext);
        }
    }
}