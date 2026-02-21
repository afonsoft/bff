
using Newtonsoft.Json;

namespace Eaf.Template.Bff.Core.Models
{
    /// <summary>
    /// This is a default api response without type of return
    /// </summary>
    public class ResponseBase : ApiResponse<ResponseBase>
    {
        /// <summary>
        /// Initializes a new instance of the ResponseBase class
        /// </summary>
        public ResponseBase() : base()
        {
            Success = false;
        }

        /// <summary>
        /// Initializes a new instance of the ResponseBase class with success flag and response base
        /// </summary>
        /// <param name="success">Success flag</param>
        /// <param name="responseBase">Response base object</param>
        public ResponseBase(bool success, ResponseBase responseBase)
            : base(success, responseBase)
        {
            Success = false;
        }

        /// <summary>
        /// Initializes a new instance of the ResponseBase class with success flag and error details
        /// </summary>
        /// <param name="success">Success flag</param>
        /// <param name="error">Error details</param>
        public ResponseBase(bool success, ErrorDetails error)
            : base(success, error)
        {
            Success = false;
        }
    }

    /// <summary>
    /// This is a default Api Response
    /// </summary>
    /// <typeparam name="T">Object of return</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the ApiResponse class
        /// </summary>
        public ApiResponse()
        { }

        /// <summary>
        /// Initializes a new instance of the ApiResponse class with success flag and error details
        /// </summary>
        /// <param name="success">Success flag</param>
        /// <param name="error">Error details</param>
        public ApiResponse(bool success, ErrorDetails error)
        {
            Error = error;
        }

        /// <summary>
        /// Initializes a new instance of the ApiResponse class with success flag and response object
        /// </summary>
        /// <param name="success">Success flag</param>
        /// <param name="responseBase">Response object</param>
        public ApiResponse(bool success, T responseBase)
        {
            Success = success;
            Response = responseBase;
        }

        /// <summary>
        /// Initializes a new instance of the ApiResponse class with response object
        /// </summary>
        /// <param name="responseBase">Response object</param>
        public ApiResponse(T responseBase)
        {
            Success = true;
            Response = responseBase;
        }

        /// <summary>
        /// Gets or sets the success flag
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; } = false;

        /// <summary>
        /// Gets or sets the response object
        /// </summary>
        [JsonProperty("response")]
        public T Response { get; set; } = default!;

        /// <summary>
        /// Gets or sets the error details
        /// </summary>
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public ErrorDetails? Error { get; set; }
    }

}