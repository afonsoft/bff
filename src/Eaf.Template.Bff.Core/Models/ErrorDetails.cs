using Newtonsoft.Json;

namespace Eaf.Template.Bff.Core.Models
{
    public class ErrorDetails
    {
        public ErrorDetails()
        {
        }

        public ErrorDetails(DateTime? timeStamp,
            string source,
            string exceptionType,
            string message,
            string? exception = null,
            string? additionalInformation = null)
        {
            TimeStamp = timeStamp;
            Message = message;
            Source = source;
            ExceptionType = exceptionType;
            Exception = exception;
            AdditionalInformation = additionalInformation;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        [JsonProperty("timeStamp")]
        public DateTime? TimeStamp { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("source")]
        public string Source { get; set; } = string.Empty;

        [JsonProperty("exceptionType")]
        public string ExceptionType { get; set; } = string.Empty;

        [JsonProperty("additionalInformation", NullValueHandling = NullValueHandling.Ignore)]
        public string? AdditionalInformation { get; set; }

        [JsonProperty("exception", NullValueHandling = NullValueHandling.Ignore)]
        public string? Exception { get; set; }
    }
}