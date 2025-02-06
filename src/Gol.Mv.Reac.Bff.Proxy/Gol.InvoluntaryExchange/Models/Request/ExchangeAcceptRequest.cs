using System.Text.Json.Serialization;

namespace Gol.InvoluntaryExchange.Models.Request
{
    public class ExchangeAcceptRequest
    {
        public ExchangeAcceptRequest()
        {
            criteria = new ExchangeAcceptCriteria();
        }

        [JsonPropertyName("criteria")]
        public ExchangeAcceptCriteria criteria { get; set; }
    }
}