using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Description;
using Swashbuckle.Swagger;
using Swashbuckle.Swagger.Annotations;
using Tbo.WebHost.Models;

namespace Tbo.WebHost.Filters
{
    /// <summary>
    /// Фильтр для установки всем запросам одинаковых возможных ответов
    /// </summary>
    public class SwaggerOperationResponsesFilter : IOperationFilter
    {
        private HttpStatusCode[] _codes = {HttpStatusCode.BadRequest, HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized };

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var attributes = apiDescription.ActionDescriptor.GetCustomAttributes<SwaggerResponseAttribute>();

            if (operation.responses == null)
                operation.responses = new Dictionary<string, Response>();
            foreach (var code in _codes)
            {
                var codeNum = ((int) code).ToString();
                var codeName = code.ToString();
                // добавляем описание
                if (!operation.responses.ContainsKey(codeNum))
                    operation.responses.Add(codeNum,
                        new Response {description = codeName, schema = schemaRegistry.GetOrRegister(typeof(ResponseModel)) });

                if (attributes.All(x => x.StatusCode != (int) HttpStatusCode.OK))
                {
                    operation.responses.Remove("200");
                    operation.responses.Add("200",
                        new Response { description = HttpStatusCode.OK.ToString(), schema = schemaRegistry.GetOrRegister(typeof(ResponseModel)) });
                }
            }
        }
    }
}