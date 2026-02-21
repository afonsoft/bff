using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OpenTelemetryExtensions
    {
        /// <summary>
        /// Add OpenTelemetry for IServiceCollection with comprehensive tracing, metrics, and logging
        /// </summary>
        /// <param name="services"></param>
        public static OpenTelemetryBuilder ConfigOpenTelemetry(this IServiceCollection services)
        {
            // Configure HTTP context switch for better telemetry
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // Configure logging with OpenTelemetry integration
            services.AddLogging(configure =>
            {
                configure.AddOpenTelemetry(builder =>
                {
                    builder.IncludeFormattedMessage = true;
                    builder.ParseStateValues = true;
                    builder.IncludeScopes = true;
                    builder.AddOtlpExporter();
                });
            });

            // Configure OpenTelemetry with comprehensive observability
            return services.AddOpenTelemetry()
                .ConfigureResource(builder => builder
                    .AddEnvironmentVariableDetector()
                    .AddTelemetrySdk()
                    .AddService(serviceName: "Eaf.Template.Bff.Host"))
                .WithTracing(builder =>
                {
                    // Add custom sources for application-specific tracing
                    builder.AddSource("Eaf.Template.Bff.*")
                        .AddSource("Eaf.Template.Bff.Host");
                    
                    // Configure ASP.NET Core instrumentation with exception recording
                    builder.AddAspNetCoreInstrumentation();
                    
                    // Configure Entity Framework Core instrumentation
                    builder.AddEntityFrameworkCoreInstrumentation();
                    
                    // Configure HttpClient instrumentation with detailed tracing
                    builder.AddHttpClientInstrumentation();
                    
                    // Configure runtime instrumentation
                    // builder.AddRuntimeInstrumentation(); // Commented out due to API incompatibility
                })
                .WithMetrics(builder =>
                {
                    // Add ASP.NET Core metrics
                    builder.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation();
                        // .AddRuntimeInstrumentation(); // Commented out due to API incompatibility
                    
                    // Add system metrics
                    builder.AddMeter("Microsoft.AspNetCore.Hosting")
                        .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                        .AddMeter("System.Net.Http")
                        .AddMeter("System.Net.Sockets");
                    
                    // Add application-specific metrics
                    builder.AddMeter("Eaf.Template.Bff.*")
                        .AddMeter("Eaf.Template.Bff.Host");
                    
                    // Configure exporters for metrics and traces
                    builder.AddPrometheusExporter()
                        .AddOtlpExporter();
                })
                .WithLogging(builder =>
                {
                    // Configure OpenTelemetry logging
                    builder.AddOtlpExporter();
                });
        }
    }
}