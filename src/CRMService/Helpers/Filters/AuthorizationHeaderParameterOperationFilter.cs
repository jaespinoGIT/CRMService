using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace CRMService.Helpers.Filters
{
    /// <summary>
    /// Adds token parameter option to swagger, added in swaggerconfig
    /// </summary>
    public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {          

            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                type = "string",
                required = true,
                schema = new Schema
                {
                    type = "String",
                    @default = new OpenApiString("Bearer")
                }
            });

        }
    }
}
