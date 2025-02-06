using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace Eaf.Template.Bff.Proxy.Bacen
{
    /// <summary>
    /// https://portal.febraban.org.br/pagina/3164/12/pt-br/associados
    /// </summary>
    public class FebrabanClient
    {
        private string _baseUrl;
        private readonly HttpClient _httpClient;

        public FebrabanClient(HttpClient httpClient)
        {
            BaseUrl = "https://portal.febraban.org.br/Associado/Index";
            _httpClient = httpClient;
        }

        public string BaseUrl
        {
            get { return _baseUrl; }
            set
            {
                _baseUrl = value;
                if (!string.IsNullOrEmpty(_baseUrl) && !_baseUrl.EndsWith('/'))
                    _baseUrl += '/';
            }
        }

        public async Task<List<FebrabanBank>> GetBankAsync(string? filter = "")
        {
            var json = JsonConvert.SerializeObject(new BankRequest() { Filter = filter ?? "" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl, content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync(CancellationToken.None);
            if (string.IsNullOrEmpty(jsonResponse))
                return new List<FebrabanBank>();

            return JsonConvert.DeserializeObject<BankResponse>(jsonResponse)?.BankList ?? new List<FebrabanBank>();
        }
    }

    #region reqeust

    internal class BankRequest
    {
        [JsonProperty("FiltroAssociado")]
        public string Type { get; set; } = "Sim";

        [JsonProperty("Busca")]
        public string? Filter { get; set; } = "";
    }

    #endregion reqeust

    #region response

    internal class BankResponse
    {
        [JsonProperty("listaBancos")]
        public List<FebrabanBank> BankList { get; set; } = new List<FebrabanBank>();

        [JsonProperty("TituloTabela")]
        public string? Title { get; set; }
    }

    public class FebrabanBank
    {
        [JsonProperty("id_banco")]
        public int Id { get; set; }

        [JsonProperty("banco")]
        public string? Name { get; set; }

        [JsonProperty("Compensacao")]
        public string? Compensation { get; set; }

        [JsonProperty("idBacen")]
        public string? IdBacen { get; set; }
    }

    #endregion response
}