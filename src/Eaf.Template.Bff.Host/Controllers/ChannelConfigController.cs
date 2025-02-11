using Eaf.Template.Bff.Core.Models;
using Eaf.Template.Bff.Core.Services.Gol.ChannelConfiguration;
using Eaf.Template.Bff.Proxy.Gol.ChannelConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eaf.Template.Bff.Host.Controllers
{
    [AllowAnonymous]
    [Route("api/ChannelConfig/[action]")]
    public class ChannelConfigController : BaseController
    {
        private readonly ChannelCfgService _channelCfg;

        public ChannelConfigController(ChannelCfgService channelCfg)
        {
            _channelCfg = channelCfg;
        }

        [HttpGet]
        [ProducesResponseType(500, Type = typeof(ResponseBase))]
        [ProducesResponseType(200, Type = typeof(ICollection<KeyDescriptionDTO>))]
        [SwaggerOperation(OperationId = "ChannelConfig", Tags = new string[] { "Get" })]
        public async Task<ActionResult<ApiResponse<ICollection<KeyDescriptionDTO>>>> Get()
        {
            return CustomResponse(await _channelCfg.ApplicationListAllAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(500, Type = typeof(ResponseBase))]
        [ProducesResponseType(200, Type = typeof(KeyListDTO))]
        [SwaggerOperation(OperationId = "ChannelConfig", Tags = new string[] { "Get" })]
        public async Task<ActionResult<ApiResponse<KeyListDTO>>> Get(string id)
        {
            return CustomResponse(await _channelCfg.ApplicationListAsync(id));
        }
    }
}