using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using FileManager.Core.Interfaces;
using Tbo.WebHost.Controllers.Api.Common;

namespace Tbo.WebHost.Controllers.Api.File
{
    /// <summary>
    /// Контроллер для файлового обмена
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/fileapi")]
    public class FileApiController : BaseApiController
    {
        private readonly IFileManager _fileManager;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="fileManager"></param>
        public FileApiController(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        /// <summary>
        /// Загрузка файла
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post()
        {
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                var file = httpRequest.Files[httpRequest.Files.Keys[0]];

                return Success(_fileManager.Create(file.FileName, file.InputStream).Id);

            }
            return Failure("Не передан файл");
        }


        /// <summary>
        /// Загрузка файла
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public HttpResponseMessage Get(long id)
        {
            var file = _fileManager.GetData(id);

            if (file == null)
                return Failure("Файл не найден", HttpStatusCode.NotFound);

            var bytes = file.FileData;
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)
            };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = file.FileInfo.FullName
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return response;
        }
    }
}
