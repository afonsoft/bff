using Eaf.Template.Bff.Proxy.ProfileSync;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Configuration;

namespace Eaf.Template.Bff.Core.Services.ProfileSync
{
    public class ProfileSyncService
    {
        private readonly ProfileSyncClient _client;
        private readonly ICacheManager _cacheManager;

        public ProfileSyncService(ProfileSyncClient client, IConfiguration configuration, ICacheManager cacheManager)
        {
            _client = client;
            _client.BaseUrl = configuration["API_URL_PROFILESYNC"] ?? "";
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// Recuperar o Loyalty do passageiro
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<LoyaltyDTO> SearchLoyaltiesByCpf(string cpf)
        {
            cpf = cpf.Replace("-", "").Replace(".", "").Trim();

            if (!IsCpf(cpf))
                throw new ArgumentOutOfRangeException(nameof(cpf), "Invalid CPF");

            //Working with Cache (IDistributedCache), if is null or expiate create new cache from action
            return await _cacheManager.GetOrCreateAsync<LoyaltyDTO>($"LoyaltyCache_{cpf}", async () =>
            {
                var profile = await _client.SearchAsync(ProfileType._2, null, cpf, null, null, null, null, null, null, null, null);

                if (profile == null || profile.Data?.Count <= 0)
                    return new LoyaltyDTO();

                var client = await _client.ProfilesGETAsync(profile.Data.First().ProfileUniqueId);

                if (client == null || client.Response?.Loyalties?.Count <= 0)
                    return new LoyaltyDTO();

                return client.Response.Loyalties.FirstOrDefault();
            });
        }

        private bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}