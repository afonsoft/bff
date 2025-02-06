using Eaf.Template.Bff.Proxy.Gol.ChannelConfiguration;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Configuration;

namespace Eaf.Template.Bff.Core.Services.Gol.ChannelConfiguration
{
    public class ChannelCfgService
    {
        private readonly ChannelCfgClient _client;
        private readonly ICacheManager _cacheManager;

        public ChannelCfgService(ChannelCfgClient client, IConfiguration configuration, ICacheManager cacheManager)
        {
            _client = client;
            _client.BaseUrl = configuration["API_URL_CHANNEL"] ?? "";
            _cacheManager = cacheManager;
        }

        public async Task<ICollection<KeyDescriptionDTO>> ApplicationListAllAsync()
        {
            //Working with Cache (IDistributedCache), if is null or expiate create new cache from action
            return await _cacheManager.GetOrCreateAsync<ICollection<KeyDescriptionDTO>>("ApplicationListAllCache", async () => await _client.ApplicationListAllAsync());
        }

        public async Task<KeyListDTO> ApplicationListAsync(string id)
        {
            return await _cacheManager.GetOrCreateAsync<KeyListDTO>($"ApplicationListCache_{id}", async () => await _client.ApplicationListGETAsync(id));
        }
    }
}