using Eaf.Template.Bff.Core.Models;
using Eaf.Template.Bff.Core.Services.ProfileSync;
using Eaf.Template.Bff.Proxy.ProfileSync;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Eaf.Template.Bff.Host.Controllers
{
    [Route("api/[controller]")]
    public class ProfileSyncController : BaseController
    {
        private readonly ProfileSyncService _profilesync;

        public ProfileSyncController(ProfileSyncService profilesync)
        {
            _profilesync = profilesync;
        }

        [Authorize]
        [HttpGet("SearchLoyaltiesByCpf/{cpf}")]
        [ProducesResponseType(500, Type = typeof(ResponseBase))]
        [ProducesResponseType(400, Type = typeof(ResponseBase))]
        [ProducesResponseType(200, Type = typeof(LoyaltyDTO))]
        [SwaggerOperation(OperationId = "ProfileSync", Tags = new string[] { "SearchLoyaltiesByCpf" })]
        public async Task<ActionResult<ApiResponse<LoyaltyDTO>>> Get(string cpf)
        {
            return CustomResponse(await _profilesync.SearchLoyaltiesByCpf(cpf));
        }
    }
}