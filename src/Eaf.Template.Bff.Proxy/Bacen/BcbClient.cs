using Newtonsoft.Json;
using System.Text;

namespace Eaf.Template.Bff.Proxy.Bacen
{
    /// <summary>
    /// https://www.bcb.gov.br/meubc/encontreinstituicao
    /// </summary>
    public class BcbClient
    {
        private string _baseUrl = string.Empty;
        private readonly HttpClient _httpClient;

        public BcbClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            BaseUrl = "https://www3.bcb.gov.br/informes/rest/pessoasJuridicas";
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

        public async Task<List<BcbBank>> GetBankAsync(string? filter = "")
        {
            var json = JsonConvert.SerializeObject(new BcbReqeust() { Nome = filter ?? "" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(BaseUrl, content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync(CancellationToken.None);
            if (string.IsNullOrEmpty(jsonResponse))
                return new List<BcbBank>();

            return JsonConvert.DeserializeObject<BcbResponse>(jsonResponse)?.Content ?? new List<BcbBank>();
        }
    }

    #region request

    internal class BcbReqeust
    {
        [JsonProperty("segmento")]
        public SegmentoRq Segmento { get; set; } = new SegmentoRq();

        [JsonProperty("nome")]
        public string Nome { get; set; } = "";

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; } = "";

        [JsonProperty("pais")]
        public string Pais { get; set; } = "Brasil";

        [JsonProperty("estado")]
        public object? Estado { get; set; } = null;

        [JsonProperty("municipio")]
        public object? Municipio { get; set; } = null;

        [JsonProperty("incluirInstituicoesLiquidacao")]
        public bool IncluirInstituicoesLiquidacao { get; set; } = false;

        [JsonProperty("incluirAgencias")]
        public bool IncluirAgencias { get; set; } = false;

        [JsonProperty("tamanhoPagina")]
        public int TamanhoPagina { get; set; } = 2000;

        [JsonProperty("numeroPagina")]
        public int NumeroPagina { get; set; } = 0;
    }

    internal class SegmentoRq
    {
        [JsonProperty("id")]
        public int Id { get; set; } = 8;

        [JsonProperty("nome")]
        public string Nome { get; set; } = "Banco Múltiplo";

        [JsonProperty("ativo")]
        public bool Ativo { get; set; } = true;
    }

    #endregion request

    #region response

    public class BcbBank
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string? Name { get; set; }

        [JsonProperty("codigoCompensacao")]
        public string? Compensation { get; set; }

        [JsonProperty("idBacen")]
        public string? IdBacen { get; set; }
    }

    internal class BcbResponse
    {
        [JsonProperty("content")]
        public List<BcbBank> Content { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("totalElements")]
        public int TotalElements { get; set; }

        [JsonProperty("last")]
        public bool Last { get; set; }

        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("numberOfElements")]
        public int NumberOfElements { get; set; }

        [JsonProperty("sort")]
        public object Sort { get; set; }

        [JsonProperty("first")]
        public bool First { get; set; }
    }

    #endregion response
}