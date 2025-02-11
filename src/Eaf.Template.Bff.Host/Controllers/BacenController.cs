using Eaf.Template.Bff.Core.Models;
using Eaf.Template.Bff.Core.Services.Bacen;
using Eaf.Template.Bff.Core.Services.Bacen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eaf.Template.Bff.Host.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BacenController : BaseController
    {
        private readonly BacenService _bacenService;

        public BacenController(BacenService bacenService)
        {
            _bacenService = bacenService;
        }

        [HttpGet]
        [ProducesResponseType(500, Type = typeof(ResponseBase))]
        [ProducesResponseType(200, Type = typeof(ApiResponse<ICollection<BankDto>>))]
        public async Task<ActionResult<ApiResponse<ICollection<BankDto>>>> Get([FromQuery] string? filter = "")
        {
            return CustomResponse(await _bacenService.GetBanksAsync(filter));
        }
    }
}