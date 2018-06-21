using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Tbo.WebHost.Filters
{
    /// <summary>
    /// Фильтр для установки всем запросам одинаковых возможных ответов
    /// </summary>
    public class SwaggerMobileDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {

            swaggerDoc.paths = swaggerDoc.paths.Where(x => x.Key.Contains("mobile")).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}