using Gol.InvoluntaryExchange.Models.Request;
using Gol.InvoluntaryExchange.Models.Response;
using Eaf.Template.Bff.Core.Exceptions;
using Eaf.Template.Bff.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Swashbuckle.AspNetCore.Annotations;
using Eaf.Template.Bff.Core.Services.InvoluntaryExchange;
using Microsoft.AspNetCore.Http.Timeouts;

namespace Eaf.Template.Bff.Host.Controllers
{
    [Route("api/exchange/[action]")]
    public class InvoluntaryExchangeController : BaseController
    {
        private readonly InvoluntaryExchangeService _involuntaryExchangeService;

        public InvoluntaryExchangeController(InvoluntaryExchangeService involuntaryExchangeService)
        {
            _involuntaryExchangeService = involuntaryExchangeService;
        }

        [RequestTimeout(milliseconds: 60000)] //60 second
        [Authorize]
        [HttpPost]
        [ActionName("accept")]
        [ProducesResponseType(200, Type = typeof(ExchangeAcceptResponse))]
        [ProducesResponseType(400, Type = typeof(ResponseBase))]
        [ProducesResponseType(500, Type = typeof(ResponseBase))]
        [SwaggerOperation(OperationId = "InvoluntaryExchange", Tags = new string[] { "Accept" })]
        public async Task<ActionResult<ApiResponse<ExchangeAcceptResponse>>> GetInvoluntaryExchangeAccept([FromBody] ExchangeAcceptBaseRequest<ExchangeAcceptRequest> request)
        {
            try
            {
                Claim dctClaims = User?.Claims?.FirstOrDefault(c => c.Type == "dct") ?? GetClaim("dct");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _involuntaryExchangeService.GetInvoluntaryExchangeAccept(request, dctClaims.Value).ConfigureAwait(false);

                if (response.Success)
                    return CustomResponse(response.Response, true);
                else
                    return CustomResponse(response.Error, false);
            }
            catch (TaskCanceledException exception)
            {
                if (exception.Source == "System.Net.Http")
                    throw new BackendInfrasctructureException("Request timed out", exception);
                else
                    throw new UnexpectedException("Unexpected Timeout.", exception);
            }
            catch (Exception exception)
            {
                if (exception?.InnerException?.InnerException?.Message != null)
                    return CustomResponse(System.Net.HttpStatusCode.BadRequest, false);
                else
                    throw new UnexpectedException("Unexpected error! " + exception.Message, null);
            }
        }
    }
}