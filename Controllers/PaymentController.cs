using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stormpath_angularjs_dotnet_stripe_twilio.Models;
using stormpath_angularjs_dotnet_stripe_twilio.Services;

namespace stormpath_angularjs_dotnet_stripe_twilio.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        private readonly AccountService _accountService;
        
        public PaymentController(PaymentService paymentService, AccountService accountService)
        {
            _paymentService = paymentService;
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentFormData formData)
        {
            if (!_paymentService.ProcessPayment(formData.Token, PaymentService.DepositAmount))
            {
                return BadRequest();
            }

            await _accountService.UpdateUserBalance(PaymentService.DepositAmount);
            var updatedAccountInfo = await _accountService.GetUserAccountInfo();
            return Ok(updatedAccountInfo);
        }
    }
}
