using Eaf.Template.Bff.Proxy.Bacen;
using Eaf.Template.Bff.Proxy.Gol.ChannelConfiguration;
using Eaf.Template.Bff.Proxy.Gol.InvoluntaryExchange;
using Eaf.Template.Bff.Proxy.ProfileSync;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Serilog;
using System.Net;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientWithPollyExtensions
    {
        /// <summary>
        /// Configure AddResiliencePipeline 'Default' and AddStandardResilienceHandler key with <see cref="CircuitBreakerStrategyOptions"/> and <see cref="RetryStrategyOptions" />
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns></returns>
        public static void ConfigHttpClientWithPolly(this IServiceCollection services, IConfiguration configuration)
        {
            #region StrategyOptions

            var retry = new Http.Resilience.HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 2,
                Delay = TimeSpan.FromMilliseconds(300),
                MaxDelay = TimeSpan.FromSeconds(1),
                ShouldHandle = arguments => arguments.Outcome switch
                {
                    { Exception: HttpRequestException } => PredicateResult.True(),
                    { Result: HttpResponseMessage response } when response.StatusCode == HttpStatusCode.InternalServerError => PredicateResult.True(),
                    _ => PredicateResult.False(),
                },
                OnRetry = arguments =>
                {
                    Log.Logger.Warning("[RESILIENCE][RETRY]Retry Attempt Number {0} after {1} seconds", arguments.AttemptNumber, arguments.RetryDelay.TotalSeconds);
                    return default;
                }
            };

            var circuitBreaker = new Http.Resilience.HttpCircuitBreakerStrategyOptions
            {
                BreakDuration = TimeSpan.FromSeconds(10),
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromMinutes(10),
                MinimumThroughput = 5,
                BreakDurationGenerator = static args => new ValueTask<TimeSpan>(TimeSpan.FromMinutes(args.FailureCount)),
                ShouldHandle = arguments => arguments.Outcome switch
                {
                    { Exception: HttpRequestException } => PredicateResult.True(),
                    { Result: HttpResponseMessage response } when response.StatusCode == HttpStatusCode.InternalServerError => PredicateResult.True(),
                    _ => PredicateResult.False(),
                }
            };

            var attemptTimeout = new Http.Resilience.HttpTimeoutStrategyOptions
            {
                //Limite timeout
                Timeout = TimeSpan.FromMinutes(5),

                // Register user callback called whenever timeout occurs
                OnTimeout = _ =>
                {
                    Log.Logger.Warning("[RESILIENCE][TIMEOUT] Timeout Execution");
                    return default;
                },
            };

            #endregion StrategyOptions

            //Create a Default Pipeline
            services.AddResiliencePipeline<string, HttpResponseMessage>("Default", (builder, context) =>
            {
                builder
                .AddRetry(retry)
                .AddCircuitBreaker(circuitBreaker)
                .AddTimeout(attemptTimeout);
            });

            //Configure Resilience/Polly for ChannelCfgClient
#pragma warning disable CS8603 // Poss�vel retorno de refer�ncia nula.
            services.AddHttpClient<ChannelCfgClient>()
            .AddStandardResilienceHandler(configure =>
            {
                #region StrategyOptions

                configure.Retry = retry;
                configure.CircuitBreaker = circuitBreaker;
                configure.TotalRequestTimeout = attemptTimeout;

                #endregion StrategyOptions
            });

            services.AddHttpClient<FebrabanClient>()
                .AddStandardResilienceHandler(configure =>
                {
                    #region StrategyOptions

                    configure.Retry = retry;
                    configure.CircuitBreaker = circuitBreaker;
                    configure.TotalRequestTimeout = attemptTimeout;

                    #endregion StrategyOptions
                });

            services.AddHttpClient<BcbClient>()
                .AddStandardResilienceHandler(configure =>
                {
                    #region StrategyOptions

                    configure.Retry = retry;
                    configure.CircuitBreaker = circuitBreaker;
                    configure.TotalRequestTimeout = attemptTimeout;

                    #endregion StrategyOptions
                });

            services.AddHttpClient<ProfileSyncClient>()
                .AddStandardResilienceHandler(configure =>
                {
                    #region StrategyOptions

                    configure.Retry = retry;
                    configure.CircuitBreaker = circuitBreaker;
                    configure.TotalRequestTimeout = attemptTimeout;

                    #endregion StrategyOptions
                });

            services.AddHttpClient<InvoluntaryExchangeClient>();
           
#pragma warning disable CS8603 // Poss�vel retorno de refer�ncia nula.
        }
    }
}