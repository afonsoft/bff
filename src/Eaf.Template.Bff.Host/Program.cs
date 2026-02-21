using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Eaf.Template.Bff.Host
{
    public class Program
    {
        private const string prefix = "BFF_";

        public static void Main(string[] args)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

                Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}][{ThreadId}] {Message:lj} {Exception} {NewLine}")
                .CreateBootstrapLogger();

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Fatal Error in Main : {0}", ex.Message);
                Environment.Exit(1);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseSerilog((context, services, configuration) =>
            {
                configuration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .ReadFrom.Services(services)
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironment("ASPNETCORE_ENVIRONMENT")
                .Enrich.WithProperty("Organization", "TEMPLATE")
                .Enrich.WithProcessName()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}][{ThreadId}] {Message:lj} {Exception} {NewLine}");
            })
            .ConfigureAppConfiguration((ctx, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddInMemoryCollection();
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
                config.AddEnvironmentVariables(prefix: prefix);

                Console.WriteLine($"Current Culture:{CultureInfo.CurrentCulture.Name} - {CultureInfo.CurrentCulture.DisplayName} - {CultureInfo.CurrentCulture.NativeName}");
                Console.WriteLine($"Current TimeZone Local: {TimeZoneInfo.Local.Id} - {TimeZoneInfo.Local.DisplayName} - {TimeZoneInfo.Local.StandardName}");

                Console.WriteLine("Environment Variables:");
                foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
                {
                    if (de.Key != null &&
                    (de.Key.ToString().Contains(prefix)
                        || de.Key.ToString().ToLower().Contains("configuration")
                        || de.Key.ToString().ToLower().Contains("environment")
                        || de.Key.ToString().ToLower().Contains("proxy")))
                    {
                        Console.WriteLine("{0} = {1}", de.Key, de.Value);
                    }
                }
            })

            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel(opt =>
                    {
                        opt.AllowSynchronousIO = true;
                        opt.AddServerHeader = false;
                        opt.Limits.MaxRequestLineSize = 16 * 1024;
                        opt.Limits.MaxRequestBodySize = int.MaxValue;
                    })
                .UseStartup<Startup>();
            });
    }
}