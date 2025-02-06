using Gol.InvoluntaryExchange.Models.Request;
using Gol.InvoluntaryExchange.Models.Response;
using Newtonsoft.Json;
using System.Text;

namespace Eaf.Template.Bff.Proxy.Gol.InvoluntaryExchange
{
    public class InvoluntaryExchangeClient
    {
        private readonly HttpClient _httpClient;

        private string _baseUrl;

        public InvoluntaryExchangeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
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

        public async Task<ExchangeAcceptResponse> GetInvoluntaryExchangeAcceptResponse(ExchangeAcceptBaseRequest<ExchangeAcceptRequest> request, string claim)
        {
            return await GetInvoluntaryExchangeAccept(request, claim);
        }

        private async Task<ExchangeAcceptResponse> GetInvoluntaryExchangeAccept(ExchangeAcceptBaseRequest<ExchangeAcceptRequest> request, string claim)
        {
            string host = BaseUrl + "exchange/accept";

            string url = $"{host}?claim={claim}";

            ExchangeAcceptBaseRequest<ExchangeAcceptRequest> model = CreateBodyFromRequest(request);

            string body = JsonConvert.SerializeObject(model);

            StringContent content = new StringContent(body, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            try
            {
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ExchangeAcceptResponse>(json);
            }
            catch
            {
                var error = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ExchangeAcceptResponse>(error);
            }
        }

        private ExchangeAcceptBaseRequest<ExchangeAcceptRequest> CreateBodyFromRequest(ExchangeAcceptBaseRequest<ExchangeAcceptRequest> request)
        {
            ExchangeAcceptBaseRequest<ExchangeAcceptRequest> model = new ExchangeAcceptBaseRequest<ExchangeAcceptRequest>
            {
                exchangeAccept = new ExchangeAcceptRequest
                {
                    criteria = new ExchangeAcceptCriteria
                    {
                        DepartureStation = request.exchangeAccept.criteria.DepartureStation,
                        PassengerLastName = request.exchangeAccept.criteria.PassengerLastName,
                        RecordLocator = request.exchangeAccept.criteria.RecordLocator
                    }
                }
            };

            return model;
        }
    }
}