using System.Text.Json.Serialization;

namespace Gol.InvoluntaryExchange.Models.Request
{
    public class ExchangeAcceptBaseRequest<T> where T : new()
    {
        public ExchangeAcceptBaseRequest()
        {
            exchangeAccept = new T();
        }

        [JsonPropertyName("exchangeAccept")]
        public T exchangeAccept { get; set; }
    }
}