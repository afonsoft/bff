using AutoMapper;
using Eaf.Template.Bff.Core.Services.Bacen.Models;
using Eaf.Template.Bff.Proxy.Bacen;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Eaf.Template.Bff.Core.Services.Bacen
{
    /// <summary>
    /// Service for managing Bacen (Central Bank of Brazil) related operations
    /// Implements Single Responsibility Principle by focusing only on Bacen operations
    /// Implements Dependency Inversion Principle by depending on abstractions
    /// </summary>
    public class BacenService : IBacenService
    {
        private readonly BcbClient _bcbClient;
        private readonly FebrabanClient _febrabanClient;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger<BacenService> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the BacenService class
        /// </summary>
        /// <param name="bcbClient">BCB client for bank operations</param>
        /// <param name="febrabanClient">Febraban client for bank operations</param>
        /// <param name="mapper">AutoMapper for object mapping</param>
        /// <param name="logger">Logger for error tracking</param>
        /// <param name="configuration">Application configuration</param>
        /// <param name="cacheManager">Cache manager for performance optimization</param>
        public BacenService(BcbClient bcbClient, FebrabanClient febrabanClient, IMapper mapper, ILogger<BacenService> logger, IConfiguration configuration, ICacheManager cacheManager)
        {
            _bcbClient = bcbClient;
            _febrabanClient = febrabanClient;
            _mapper = mapper;
            _logger = logger;

            _bcbClient.BaseUrl = configuration["API_URL_BCB"] ?? "";
            _febrabanClient.BaseUrl = configuration["API_URL_FEBRABAN"] ?? "";
            _cacheManager = cacheManager;
        }

        private static DistributedCacheEntryOptions _distributedCacheEntryOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromHours(6), //Expira depois de 6h sem usar o cache
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)// expira depois de 24h do cache criado
        };

        /// <summary>
        /// Retrieves a list of banks from Bacen APIs with fallback mechanism
        /// Implements Circuit Breaker pattern by trying Febraban first, then BCB
        /// </summary>
        /// <param name="filter">Optional filter for bank search</param>
        /// <returns>List of bank DTOs</returns>
        public async Task<List<BankDto>> GetBanksAsync(string filter = "")
        {
            var innerExceptions = new List<Exception>();

            try
            {
                // Try Febraban first (primary source)
                var febrabanBanks = await _cacheManager.GetOrCreateAsync($"febrabanClientCache_{filter}", _distributedCacheEntryOptions, async () => await _febrabanClient.GetBankAsync(filter));
                return _mapper.Map<List<BankDto>>(febrabanBanks);
            }
            catch (Exception e)
            {
                innerExceptions.Add(e);
                _logger.LogError(e, "Error on febrabanClient.GetBankAsync");
            }

            try
            {
                // Fallback to BCB (secondary source)
                var bcbBanks = await _cacheManager.GetOrCreateAsync($"bcbClientCache_{filter}", _distributedCacheEntryOptions, async () => await _bcbClient.GetBankAsync(filter));
                return _mapper.Map<List<BankDto>>(bcbBanks);
            }
            catch (Exception e)
            {
                innerExceptions.Add(e);
                _logger.LogError(e, "Error on bcbClient.GetBankAsync");
            }

            throw new AggregateException("BacenService.GetBanksAsync()", innerExceptions);
        }
    }

    /// <summary>
    /// Interface for Bacen service operations
    /// Implements Dependency Inversion Principle
    /// </summary>
    public interface IBacenService
    {
        /// <summary>
        /// Retrieves a list of banks from Bacen APIs
        /// </summary>
        /// <param name="filter">Optional filter for bank search</param>
        /// <returns>List of bank DTOs</returns>
        Task<List<BankDto>> GetBanksAsync(string filter = "");
    }
}