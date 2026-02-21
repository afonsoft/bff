using Eaf.Template.Bff.Core.Services.Bacen;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Linq;

namespace Eaf.Template.Bff.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //MVC
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<SerilogMvcLoggingAttribute>();
                options.Filters.Add(new ResponseCacheAttribute() { NoStore = true, Location = ResponseCacheLocation.None });
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            //Configure CORS for angular2 UI
            services.AddCors(options =>
            {
                options.AddPolicy("Eaf.Template.Bff.Host", builder =>
                {
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .Build();
                });
            });

            //Configure Authentication
            services.ConfigAuthentication(Configuration);

            // Configure Dependency Injection, Health, Telemetry and Polly
            services.ConfigDependencyInjection(Configuration);
            services.ConfigOpenTelemetry();
            services.ConfigHealthChecks();
            //Configure the HttpClient
            services.ConfigHttpClientWithPolly(Configuration);

            //Configure IDistributedCache
            services.ConfigDistributedCache(Configuration);

            // Configure OpenTelemetry for tracing, metrics, and logging
            services.ConfigOpenTelemetry(); // Using existing extension from Core

            //Configure Swagger
            services.AddSwaggerGen();

            //Default timeout request 60 Seconds
            services.AddRequestTimeouts(opts => opts.DefaultPolicy = new RequestTimeoutPolicy { Timeout = TimeSpan.FromMicroseconds(60) });

            // Add Client and Service to Scoped
            // Implements Dependency Inversion Principle by registering interface instead of concrete class
            services.AddScoped<IBacenService, BacenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseCors("Eaf.Template.Bff.Host"); //Enable CORS!
            app.UseHealthChecks("/health"); //Configure HealthCheck endpoint and Port
            app.UseSerilogConfig();
            app.UseWebSockets(new WebSocketOptions { KeepAliveInterval = TimeSpan.FromMinutes(5) });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eaf.Template.Bff.Host v1"));
            }

            app.UseHsts();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseRequestTimeouts();

            //Handler Error
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                //Comment for not export url
                //endpoints.MapPrometheusScrapingEndpoint("/metrics"); //Exporter Metrics Url of OpenTelemetry
                endpoints.MapControllers();
            });
        }
    }
}