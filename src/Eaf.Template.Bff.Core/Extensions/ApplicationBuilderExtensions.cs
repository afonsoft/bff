using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog.AspNetCore;
using Serilog.Events;

namespace Serilog
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure UseSerilogRequestLogging in <see cref="IApplicationBuilder"/>
        /// </summary>
        public static IApplicationBuilder UseSerilogConfig(this IApplicationBuilder app, Action<RequestLoggingOptions>? configureOptions = null)
        {
            var loggingOptions = app.ApplicationServices.GetService<RequestLoggingOptions>() ?? new RequestLoggingOptions();

            if (configureOptions != null)
            {
                configureOptions.Invoke(loggingOptions);
                app.UseSerilogRequestLogging(configureOptions);
            }
            else
            {
                app.UseSerilogRequestLogging(configureOptions =>
                {
                    configureOptions.EnrichDiagnosticContext = EnrichFromRequest;
                    configureOptions.GetLevel = CustomGetLevel;
                });
            }

            return app;
        }

        private static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            if (diagnosticContext != null && httpContext != null)
            {
                if (httpContext.Connection?.RemoteIpAddress != null)
                    diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress.ToString() ?? string.Empty);
                if (httpContext.Request?.Headers != null
                    && httpContext.Request.Headers.Any()
                    && httpContext.Request.Headers["User-Agent"].FirstOrDefault() != null)
                    diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault() ?? string.Empty);
            }
        }

        private static LogEventLevel CustomGetLevel(HttpContext httpContext, double elapsedMs, Exception? ex)
        {
            if (httpContext == null)
                return LogEventLevel.Verbose;

            return ex != null || httpContext.Response.StatusCode > 499 ?
                    LogEventLevel.Error :
                    IsHealthCheckEndpoint(httpContext, elapsedMs);
        }

        private static LogEventLevel IsHealthCheckEndpoint(HttpContext ctx, double elapsedMs)
        {
            try
            {
                if (ctx.Request?.Path != null)
                {
                    string path = (ctx.Request.Path.Value ?? string.Empty).Split("/").LastOrDefault() ?? string.Empty;
                    switch (path)
                    {
                        case "ping":
                        case "live":
                        case "ready":
                        case "signalr":
                        case "signalr-chat":
                        case "v1":
                        case "v2":
                        case "v3":
                        case "swagger":
                        case "hangfire":
                            return LogEventLevel.Debug;
                    }
                    if (path != null && path.StartsWith("signalr"))
                        return LogEventLevel.Debug;
                }

                var endpoint = ctx.GetEndpoint();
                if (endpoint is object
                    && string.Equals(endpoint.DisplayName,
                        "Health checks",
                        StringComparison.Ordinal))
                {
                    return LogEventLevel.Debug;
                }
            }
            catch
            {
                //Igonre
            }

            if (elapsedMs > 7000)
                return LogEventLevel.Warning;
            else if (elapsedMs > 4000)
                return LogEventLevel.Debug;

            return LogEventLevel.Verbose;
        }
    }
}