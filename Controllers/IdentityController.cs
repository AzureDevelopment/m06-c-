using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{

    [Route("identity")]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer,aad-2", Policy = "OnlyWeirdScope")]
        [Route("jwt-scope")]
        public IActionResult JwtScope()
        {
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer,aad-2", Policy = "OnlyTestGroup")]
        [Route("jwt-group")]
        public IActionResult JwtGroup()
        {
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer,aad-2")]
        [Route("jwt")]
        public IActionResult Jwt()
        {
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet]

        [Authorize(AuthenticationSchemes = "Bearer,aad-1")]
        [Route("oidc")]
        public IActionResult Oidc()
        {
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet]

        [Authorize(AuthenticationSchemes = "Bearer,aad-3")]
        [Route("b2c")]
        public IActionResult OidcB2c()
        {
            return new JsonResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}