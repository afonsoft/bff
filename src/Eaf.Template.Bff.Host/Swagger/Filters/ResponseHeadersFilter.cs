using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Eaf.Template.Bff.Host.Swagger.Filters
{
    /// <summary>
    /// Swagger Filter to add HeaderSetCookie class headers
    /// </summary>
    public class ResponseHeadersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.DeclaringType.Name != "BacenController"
                 && context.MethodInfo.DeclaringType.Name != "ChannelConfigController") // ignore methods
            {
                // Get all response header declarations for a given operation
                ProducesResponseHeaderAttribute[] actionResponsesWithHeaders = context.ApiDescription.CustomAttributes()
                .OfType<ProducesResponseHeaderAttribute>()
                .ToArray();

                if (!actionResponsesWithHeaders.Any())
                    return;

                foreach (string responseCode in operation.Responses.Keys)
                {
                    // Do we have one or more headers for the specific response code
                    var responseHeaders = actionResponsesWithHeaders.Where(resp => resp.StatusCode.ToString() == responseCode);
                    if (!responseHeaders.Any())
                        continue;

                    OpenApiResponse response = operation.Responses[responseCode];

                    if (response.Headers == null)
                        response.Headers = new Dictionary<string, OpenApiHeader>();

                    foreach (var responseHeader in responseHeaders)
                    {
                        response.Headers[responseHeader.Name] = new OpenApiHeader
                        {
                            //Type = responseHeader.Type.ToString(),
                            Description = responseHeader.Description
                        };
                    }
                }
            }
        }
    }
}