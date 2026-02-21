using Eaf.Template.Bff.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Eaf.Template.Bff.Host.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BaseController : ControllerBase
    {
        protected ActionResult CustomResponse(object response = null, bool success = true)
        {
            ApiResponse<object> apiResponse = new ApiResponse<object>
            {
                Response = response,
                Success = success
            };

            apiResponse.Success = success;

            //AddHttpResponseHeader("x-sabre-cookie-encoded", GetXSabreCookiesEncoded());

            return Ok(apiResponse);
        }



        /// <summary>
        /// Get claim from authenticated user
        /// </summary>
        /// <param name="type">Type of claim to retrieve</param>
        /// <returns>Claim if found, null otherwise</returns>
        protected Claim GetClaim(string type = "oac")
        {
            if (User?.Claims != null)
                return User.Claims.FirstOrDefault(p => p.Type == type);

            string token = GetAuthorization();
            if (string.IsNullOrEmpty(token))
                return null;

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken.Claims.FirstOrDefault(p => p.Type == type);
        }

        protected string GetAuthorization()
        {
            string token = Request.Headers[HeaderNames.Authorization];
            if (string.IsNullOrEmpty(token))
                return null;
            return token.Replace("Bearer ", "").Trim();
        }


        protected void AddHttpResponseHeader(string key, string value)
        {
            if (!string.IsNullOrEmpty(value) && !Response.Headers.ContainsKey(key))
            {
                Response.Headers.Append(key, value);
            }
        }

        protected void AddHttpResponseHeader(string key, string[] value)
        {
            if (!Response.Headers.ContainsKey(key))
            {
                Response.Headers.Append(key, new Microsoft.Extensions.Primitives.StringValues(value.ToArray()));
            }
        }

        protected string RemoveInvalidCharacterFromOtherServiceInformation(string otherServiceInformation)
        {
            string invalidCharacter = "\n";

            if (!String.IsNullOrEmpty(otherServiceInformation))
            {
                otherServiceInformation = otherServiceInformation.Replace(invalidCharacter, "");
                return otherServiceInformation;
            }

            return otherServiceInformation;
        }
    }
}