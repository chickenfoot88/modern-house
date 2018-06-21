using System;
using System.Net;
using Swashbuckle.Swagger.Annotations;
using Tbo.WebHost.Models;

namespace Tbo.WebHost.Attributes
{
    public class ResponseTypeAttribute : SwaggerResponseAttribute
    {
        public ResponseTypeAttribute(HttpStatusCode statusCode) : base(statusCode)
        {
        }

        public ResponseTypeAttribute(HttpStatusCode statusCode, string description = null, Type type = null) : base(statusCode, description, type)
        {
        }

        public ResponseTypeAttribute(int statusCode) : base(statusCode)
        {
        }

        public ResponseTypeAttribute(int statusCode, string description = null, Type type = null) : base(statusCode, description, type)
        {
        }

        public ResponseTypeAttribute(Type t) : base(HttpStatusCode.OK, "Success result", typeof(ResponseModel<>).MakeGenericType(t))
        {
        }
    }
}