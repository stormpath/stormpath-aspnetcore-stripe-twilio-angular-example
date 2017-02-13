using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stormpath_angularjs_dotnet_stripe_twilio.Services;

namespace stormpath_angularjs_dotnet_stripe_twilio.Controllers
{
    [Authorize]   
    [Route("api/[controller]")]
    public class MeController : Controller
    {
        private readonly AccountService _accountService;

        public MeController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userAccountInfo = await _accountService.GetUserAccountInfo();

            return Ok(userAccountInfo);
        }

    }
}
