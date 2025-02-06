using Eaf.Template.Bff.Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Text;
using System.Text.RegularExpressions;
using Eaf.Template.Bff.Core.Exceptions;

namespace Microsoft.AspNetCore.Http
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.Error(exception, $"{exception.GetType().Name} captured.");

            context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            bool showExceptionDetails = _configuration.GetValue<bool?>("ShowExceptionDetails") ?? false;

            ErrorDetails errorDetails = null;

            if (exception is BackendBusinessRuleException)
            {
                BackendBusinessRuleException backendBusinessRuleException = (BackendBusinessRuleException)exception;
                errorDetails = new ErrorDetails(backendBusinessRuleException.TimeStamp, backendBusinessRuleException.Source, backendBusinessRuleException.GetType().ToString(), backendBusinessRuleException.Message,
                    (showExceptionDetails) ? ExceptionExtension.FormatException(backendBusinessRuleException) : null, GetAdditionalErrorInfo(backendBusinessRuleException));
            }
            else if (exception is BackendInfrasctructureException)
            {
                BackendInfrasctructureException backendInfrasctructureException = (BackendInfrasctructureException)exception;
                errorDetails = new ErrorDetails(backendInfrasctructureException.TimeStamp, backendInfrasctructureException.Source, backendInfrasctructureException.GetType().ToString(), backendInfrasctructureException.Message,
                    (showExceptionDetails) ? ExceptionExtension.FormatException(backendInfrasctructureException) : null, GetAdditionalErrorInfo(backendInfrasctructureException));
            }
            else if (exception is TaskCanceledException)
            {
                LoggerMessage("Timeout Error. ", GetAdditionalErrorInfo(exception), exception.Message);

                if (exception.Source == "System.Net.Http")
                    throw new BackendInfrasctructureException("Request timed out while waiting for a reply from Sabre Digital Connect.", exception);
                else
                    throw new UnexpectedException("Unexpected Timeout.", exception);
            }
            else
            {
                LoggerMessage("Exception not handled ", GetAdditionalErrorInfo(exception), exception.Message);

                errorDetails = new ErrorDetails(DateTime.UtcNow, exception.Source, exception.GetType().ToString(),
                    exception.Message, (showExceptionDetails) ? ExceptionExtension.FormatException(exception) : null,
                    GetAdditionalErrorInfo(exception) + ". Exception not handled.");
            }

            ApiResponse<ResponseBase> response = new ApiResponse<ResponseBase>(false, errorDetails);

            string result = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(result);
        }



        private string GetAdditionalErrorInfo(Exception exception)
        {
            string source = exception.Source;
            string methodName = exception.TargetSite.DeclaringType.Name;

            if (methodName.Contains("<") && methodName.Contains(">"))
            {
                // Get methodName between <> from DeclaringType.Name
                Regex regex = new Regex(@"(?<=\<)(.*?)(?=\>)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                methodName = regex.Match(methodName).ToString();
            }

            return "Failed to execute Method: " + methodName + " from: " + source;
        }

        private void LoggerMessage(string info, string aditionalErrorInfo, string exceptionMessage)
        {
            _logger.Error(info + " " + aditionalErrorInfo + "." + exceptionMessage);
        }
    }
}