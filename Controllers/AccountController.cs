using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Authentication.WebAssembly.Msal;

namespace IdentityServer.Controllers
{

    [Route("account")]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        [Route("login-aad-redirect")]
        public async Task<IActionResult> LoginAadRedirect(string code)
        {
            var client = new HttpClient()
            {
                BaseAddress = new System.Uri("https://login.microsoftonline.com/common/oauth2/v2.0")
            };
            var response = await client.GetAsync($"token?client_id=c8762132-e275-4aaa-bc07-d19bf317a448&scope=openid&code={code}&redirect_uri=http%3A%2F%2Flocalhost%3A6001%2Faccount%2Flogin-aad&grant_type=authorization_code&client_secret=V-C54sc2PM~5sC_J928aA3wjsh5s-MM6.t");
            return Ok(response);
        }
    }
}