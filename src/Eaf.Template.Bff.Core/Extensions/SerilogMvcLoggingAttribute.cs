using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace Serilog
{
    /// <summary>
    /// Atribute for Serilog
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SerilogMvcLoggingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var diagnosticContext = context.HttpContext.RequestServices.GetService<IDiagnosticContext>();

            if (diagnosticContext != null)
            {
                diagnosticContext.Set("ActionName", context.ActionDescriptor.DisplayName);
                diagnosticContext.Set("ActionId", context.ActionDescriptor.Id);
                diagnosticContext.Set("RouteData", context.ActionDescriptor.RouteValues);
                diagnosticContext.Set("ValidationState", context.ModelState.IsValid);
            }

            var enrichers = new List<ILogEventEnricher>
            {
                new PropertyEnricher("ActionName", context.ActionDescriptor.DisplayName),
                new PropertyEnricher("ActionId", context.ActionDescriptor.Id),
                new PropertyEnricher("RouteData", context.ActionDescriptor.RouteValues),
                new PropertyEnricher("ValidationState", context.ModelState.IsValid)
            };

            LogContext.Push(enrichers.ToArray());
        }
    }
}