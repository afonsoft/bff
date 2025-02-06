using AutoMapper;
using Eaf.Template.Bff.Core.Services.Bacen.Models;
using Eaf.Template.Bff.Proxy.Bacen;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Eaf.Template.Bff.Core.Services.Bacen
{
    public class BacenService
    {
        private readonly BcbClient _bcbClient;
        private readonly FebrabanClient _febrabanClient;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger<BacenService> _logger;
        private readonly IMapper _mapper;

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

        public async Task<List<BankDto>> GetBanksAsync(string? filter = "")
        {
            var innerExceptions = new List<Exception>();

            try
            {
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
}