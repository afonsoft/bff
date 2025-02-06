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
            string exception,
            string additionalInformation = null)
        {
            TimeStamp = timeStamp;
            Message = message;
            Source = source;
            ExceptionType = exceptionType;
            Exception = exception;
            AdditionalInformation = additionalInformation;
        }

        [JsonProperty("timeStamp")]
        public DateTime? TimeStamp { get; set; }

        [JsonProperty("message")]
        public string Message { set; get; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("exceptionType")]
        public string ExceptionType { get; set; }

        [JsonProperty("additionalInformation", NullValueHandling = NullValueHandling.Ignore)]
        public string AdditionalInformation { set; get; }

        [JsonProperty("exception", NullValueHandling = NullValueHandling.Ignore)]
        public string Exception { get; set; }
    }
}