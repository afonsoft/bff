using Eaf.Template.Bff.Proxy.Bacen;
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
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(300),
                MaxDelay = TimeSpan.FromSeconds(1),
                ShouldHandle = arguments => arguments.Outcome switch
                {
                    { Exception: HttpRequestException } => PredicateResult.True(),
                    { Result: HttpResponseMessage response } when response.StatusCode >= HttpStatusCode.InternalServerError => PredicateResult.True(),
                    _ => PredicateResult.False(),
                },
                OnRetry = arguments =>
                {
                    Log.Logger.Warning("[RESILIENCE][RETRY] Retry Attempt {0} after {1} seconds", arguments.AttemptNumber, arguments.RetryDelay.TotalSeconds);
                    return default;
                }
            };

            var circuitBreaker = new Http.Resilience.HttpCircuitBreakerStrategyOptions
            {
                BreakDuration = TimeSpan.FromSeconds(10),
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromMinutes(10),
                MinimumThroughput = 5,
                ShouldHandle = arguments => arguments.Outcome switch
                {
                    { Exception: HttpRequestException } => PredicateResult.True(),
                    { Result: HttpResponseMessage response } when response.StatusCode >= HttpStatusCode.InternalServerError => PredicateResult.True(),
                    _ => PredicateResult.False(),
                }
            };

            var timeoutStrategy = new Http.Resilience.HttpTimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromMinutes(5)
            };

            #endregion StrategyOptions

            //Create a Default Pipeline
            services.AddResiliencePipeline<string, HttpResponseMessage>("Default", (builder, context) =>
            {
                builder
                    .AddRetry(retry)
                    .AddCircuitBreaker(circuitBreaker)
                    .AddTimeout(timeoutStrategy);
            });
            
            // Configure individual clients with resilience
            services.AddHttpClient<FebrabanClient>()
                .AddStandardResilienceHandler(configure =>
                {
                    configure.Retry = retry;
                    configure.CircuitBreaker = circuitBreaker;
                    configure.TotalRequestTimeout = timeoutStrategy;
                });
            services.AddHttpClient<BcbClient>()
                .AddStandardResilienceHandler(configure =>
                {
                    configure.Retry = retry;
                    configure.CircuitBreaker = circuitBreaker;
                    configure.TotalRequestTimeout = timeoutStrategy;
                });
            
            #pragma warning disable CS8603 // Possível retorno de referência nula.
        }
    }
}