using System.Net.Http;
using System.Web.Http;
using Tbo.WebHost.Controllers.Api.Common;

namespace Tbo.WebHost.Controllers.MobileApi.Common
{
    /// <summary>
    /// общая инфа
    /// </summary>
    [RoutePrefix("api/mobile/common")]
    public class CommonController : BaseApiController
    {
        /// <summary>
        /// Получение информации о диспетчере
        /// </summary>
        [Route("manager")]
        [HttpGet]
        [Attributes.ResponseType(typeof(string))]
        public HttpResponseMessage Manager()
        {
            var key = "ManagerPhoneNumber";
            var value = System.Configuration.ConfigurationManager.AppSettings[key];

            return Success(value ?? "");
        }
    }
}
