using System.Web.Http;
using Swashbuckle.Application;
using System;
using System.Net.Http;
using Tbo.WebHost.Filters;
using Core;

namespace Tbo.WebHost
{
    /// <summary>
    /// Конфигурация Swagger
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Регистрация конфигурации
        /// </summary>
        /// <param name="config">Конфигурация api</param>
        public static void Register(HttpConfiguration config)
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            config
                .EnableSwagger(c =>
                    {
                        c.Schemes(new[] { "http" });

                        c.SingleApiVersion("v1", "ТБО");

                        c.IncludeXmlComments(GetXmlCommentsPath());
                        
                        c.UseFullTypeNameInSchemaIds();

                        c.DescribeAllEnumsAsStrings();

                        c.RootUrl(GetRootUrl);

                        c.DocumentFilter<SwaggerMobileDocumentFilter>();

                        c.OperationFilter<SwaggerOperationResponsesFilter>();
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("Web Api ТБО");

                        c.DisableValidator();

                        c.InjectJavaScript(typeof(SwaggerConfig).Assembly, "Tbo.WebHost.SwaggerExtensions.addBearerAuth.js");
                    });
        }

        private static string GetRootUrl(HttpRequestMessage msg)
        {
            return
                $"{msg.RequestUri.Scheme}://{msg.RequestUri.Host}:{msg.RequestUri.Port}{(Settings.TestEnvironment ? "" : "/test_tbo")}";
        }

        private static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\bin\Tbo.WebHost.XML", AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
