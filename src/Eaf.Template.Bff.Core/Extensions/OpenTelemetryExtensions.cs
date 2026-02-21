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
        /// Add OpenTelemetry for IServiceCollection with AspNetCoreInstrumentation, EntityFrameworkCoreInstrumentation, HangfireInstrumentation, HttpClientInstrumentation
        /// </summary>
        /// <param name="services"></param>
        public static OpenTelemetryBuilder ConfigOpenTelemetry(this IServiceCollection services)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

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

            return services.AddOpenTelemetry()
            .ConfigureResource(builder => builder
                .AddEnvironmentVariableDetector()
                .AddTelemetrySdk()
                .AddService(serviceName: "Eaf.Template.Bff.Host"))
            .WithTracing(builder =>
            {
                builder
                .AddSource("Eaf.Template.Bff.*")
                .AddSource("Eaf.Template.Bff.Host")
                .AddAspNetCoreInstrumentation(o =>
                {
                    o.RecordException = true;
                })
                .AddEntityFrameworkCoreInstrumentation()
                // .AddEntityFrameworkCoreInstrumentation(o =>
                // {
                //     o.SetDbStatementForStoredProcedure = true;
                //     o.SetDbStatementForText = true;
                // }) // API methods not available in current version
                .AddHangfireInstrumentation(o =>
                {
                    o.RecordException = true;
                })
                .AddHttpClientInstrumentation(o =>
                {
                    o.RecordException = true;
                }).AddOtlpExporter();
            })
            .WithLogging(builder =>
            {
                builder.AddOtlpExporter();
            })
            .WithMetrics(builder =>
            {
                builder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddMeter("Microsoft.AspNetCore.Hosting")
                .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
                .AddMeter("Eaf.Template.Bff.*")
                .AddMeter("Eaf.Template.Bff.Host")
                .AddPrometheusExporter()
                .AddOtlpExporter();
            });
        }
    }
}