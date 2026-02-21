using Eaf.Template.Bff.Core.Models;
using Eaf.Template.Bff.Core.Services.Bacen;
using Eaf.Template.Bff.Core.Services.Bacen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eaf.Template.Bff.Host.Controllers
{
    /// <summary>
    /// Controller for managing Bacen (Central Bank of Brazil) related operations
    /// Implements Dependency Inversion Principle by depending on IBacenService interface
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BacenController : BaseController
    {
        private readonly IBacenService _bacenService;

        /// <summary>
        /// Initializes a new instance of the BacenController class
        /// </summary>
        /// <param name="bacenService">Service interface for Bacen operations</param>
        public BacenController(IBacenService bacenService)
        {
            _bacenService = bacenService;
        }

        /// <summary>
        /// Retrieves a list of banks from Bacen API
        /// </summary>
        /// <param name="filter">Optional filter parameter for bank search</param>
        /// <returns>Collection of bank DTOs</returns>
        [HttpGet]
        [ProducesResponseType(500, Type = typeof(ResponseBase))]
        [ProducesResponseType(200, Type = typeof(ApiResponse<ICollection<BankDto>>))]
        public async Task<ActionResult<ApiResponse<ICollection<BankDto>>>> Get([FromQuery] string filter = "")
        {
            return CustomResponse(await _bacenService.GetBanksAsync(filter));
        }
    }
}