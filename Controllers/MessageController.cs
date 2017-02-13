using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using stormpath_angularjs_dotnet_stripe_twilio.Models;
using stormpath_angularjs_dotnet_stripe_twilio.Services;

namespace stormpath_angularjs_dotnet_stripe_twilio.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private readonly AccountService _accountService;
        private readonly SmsService _smsService;
        private readonly BitcoinExchangeRateService _bitcoinExchangeRateService;

        public MessageController(
            AccountService accountService,
            SmsService smsService,
            BitcoinExchangeRateService bitcoinExchangeRateService)
        {
            _accountService = accountService;
            _smsService = smsService;
            _bitcoinExchangeRateService = bitcoinExchangeRateService;
        }        

        [HttpPost]        
        public async Task<IActionResult> Post([FromBody] SendSmsRequest payload)
        {
            if(string.IsNullOrEmpty(payload.PhoneNumber))
            {
                return BadRequest("Invalid phone number");
            }

            var userAccountInfo = await _accountService.GetUserAccountInfo();

            if (userAccountInfo.Balance == 0)
            {
                return StatusCode((int)HttpStatusCode.PaymentRequired);
            }

            try
            {
                var btcExchangeRate = await _bitcoinExchangeRateService.GetBitcoinExchangeRate();
                var message = $"1 Bitcoin is currently worth ${btcExchangeRate} USD.";

                await _smsService.SendSms(message, payload.PhoneNumber);

                await _accountService.UpdateUserTotalQueries(1);
                await _accountService.UpdateUserBalance(-PaymentService.CostPerQuery);

                userAccountInfo = await _accountService.GetUserAccountInfo();
                return Ok(userAccountInfo);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
