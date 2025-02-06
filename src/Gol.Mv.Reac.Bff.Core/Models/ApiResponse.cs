
using Newtonsoft.Json;

namespace Eaf.Template.Bff.Core.Models
{
    /// <summary>
    /// This is a default api response without type of return
    /// </summary>
    public class ResponseBase : ApiResponse<ResponseBase>
    {
        public ResponseBase() : base()
        {
            Success = false;
        }

        public ResponseBase(bool success, ResponseBase responseBase)
            : base(success, responseBase)
        {
            Success = false;
        }

        public ResponseBase(bool success, ErrorDetails error)
            : base(success, error)
        {
            Success = false;
        }
    }

    /// <summary>
    /// This is a default Api Response
    /// </summary>
    /// <typeparam name="T">Object of retorn</typeparam>
    public class ApiResponse<T>
    {
        public ApiResponse()
        { }

        public ApiResponse(bool success, ErrorDetails error)
        {
            Error = error;
        }

        public ApiResponse(bool success, T responseBase)
        {
            Success = success;
            Response = responseBase;
        }

        public ApiResponse(T responseBase)
        {
            Success = true;
            Response = responseBase;
        }

        [JsonProperty("success")]
        public bool Success { get; set; } = false;

        [JsonProperty("response")]
        public T Response { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public ErrorDetails Error { get; set; }
    }

}