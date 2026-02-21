
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Eaf.Template.Bff.Host.Swagger.Filters
{
    /// <summary>
    /// Swagger Filter to remove HeaderSetCookie object from params and Add all headers from HeaderSetCookie class
    /// </summary>
    public class RequestHeadersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
           
        }
    }
}