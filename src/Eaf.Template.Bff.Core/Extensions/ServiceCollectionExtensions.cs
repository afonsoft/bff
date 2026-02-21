using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure Defaults Dependency Injection
        /// </summary>
        public static void ConfigDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddLogging();
            services.AddOptions();
            services.AddEndpointsApiExplorer();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>(); // Deprecated in .NET 8+
            services.TryAddSingleton<ICacheManager, CacheManager>();
            // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Updated to use assembly scanning

            //SignalR and Web-sockets
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.HandshakeTimeout = TimeSpan.FromMinutes(1);
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
                options.KeepAliveInterval = TimeSpan.FromMinutes(2);
            });

            //Session for MVC
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = false;
            });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            //Turn on service discovery by default
            services.AddServiceDiscovery();

            services.ConfigureHttpClientDefaults(http =>
            {
                //Turn on service discovery by default
                http.AddServiceDiscovery();
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            // Cookie configuration for HTTP to support cookies with SameSite=None
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }
    }
}