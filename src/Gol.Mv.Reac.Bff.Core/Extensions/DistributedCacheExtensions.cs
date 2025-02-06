using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// Configure IDistributedCache <see cref="https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-8.0"/>
        /// </summary>
        public static void ConfigDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            //https://learn.microsoft.com/pt-br/aspnet/core/performance/caching/memory?view=aspnetcore-9.0
            //Cache in Memory
            services.AddDistributedMemoryCache(options =>
            {
                // Set cache size limit (in bytes)
                options.SizeLimit = 1024 * 1024 * 100; // 100 MB
                // Set cache compaction percentage
                options.CompactionPercentage = 0.25; // 25%
                // Set cache expiration scan frequency
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(5); // 5 minutes
            });

            //Redis Cache
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = configuration.GetValue<string>("MyRedisConStr");
            //    options.InstanceName = "SampleInstance";
            //});

            //Sql Server Cache
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = configuration.GetValue<string>("DistCache_ConnectionString");
            //    options.SchemaName = "dbo";
            //    options.TableName = "TestCache";
            //});

            //NCache
            //services.AddNCacheDistributedCache(configuration =>
            //{
            //    configuration.CacheName = "Eaf.Template.Bff.Cache";
            //    configuration.EnableLogs = true;
            //    configuration.ExceptionsEnabled = true;
            //});

            //Cosmos DB  Cache
            //services.AddCosmosCache((CosmosCacheOptions cacheOptions) =>
            //{
            //    cacheOptions.ContainerName = Configuration["CosmosCacheContainer"];
            //    cacheOptions.DatabaseName = Configuration["CosmosCacheDatabase"];
            //    cacheOptions.CosmosClient = new CosmosClientBuilder(Configuration["CosmosConnectionString"]);
            //    cacheOptions.CreateIfNotExists = true;
            //});
        }
    }
}