using FluentValidation;
using Gol.InvoluntaryExchange.Models.Request;
using Gol.InvoluntaryExchange.Models.Response;
using Gol.InvoluntaryExchange.Validator;
using Eaf.Template.Bff.Core.Exceptions;
using Eaf.Template.Bff.Proxy.Gol.InvoluntaryExchange;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Eaf.Template.Bff.Core.Services.InvoluntaryExchange
{
    public class InvoluntaryExchangeService
    {
        private readonly ILogger<InvoluntaryExchangeService> _logger;
        private readonly InvoluntaryExchangeClient _involuntaryExchangeClient;

        public InvoluntaryExchangeService(InvoluntaryExchangeClient client, ILogger<InvoluntaryExchangeService> logger, IConfiguration configuration)
        {
            _involuntaryExchangeClient = client;
            _involuntaryExchangeClient.BaseUrl = configuration["API_URL_INVOLUNTARYEXCHANGE"] ?? "";
            _logger = logger;
        }

        public async Task<ExchangeAcceptResponse> GetInvoluntaryExchangeAccept(ExchangeAcceptBaseRequest<ExchangeAcceptRequest> request, string dctClaims)
        {
            try
            {
                await new ExchangeAcceptRequestValidation().ValidateAndThrowAsync(request.exchangeAccept);
                return await _involuntaryExchangeClient.GetInvoluntaryExchangeAcceptResponse(request, dctClaims).ConfigureAwait(false);
            }
            catch (TaskCanceledException exception)
            {
                _logger.LogWarning(exception, "TaskCanceledException");
                if (exception.Source == "System.Net.Http")
                    throw new BackendInfrasctructureException("Request timed out", exception);
                else
                    throw new UnexpectedException("Unexpected Timeout.", exception);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                if (exception?.InnerException?.InnerException?.Message != null)
                    throw new BackendInfrasctructureException(exception?.InnerException?.InnerException?.Message, null);
                else
                    throw new UnexpectedException("Unexpected error! " + exception.Message, null);
            }
        }
    }
}