using Microsoft.Extensions.Options;
using stormpath_angularjs_dotnet_stripe_twilio.Models;
using Stripe;

namespace stormpath_angularjs_dotnet_stripe_twilio.Services
{
    public class PaymentService
    {
        public static readonly int DepositAmount = 2000;
        public static readonly int CostPerQuery = 2;

        private readonly PaymentSettings _paymentSettings;
        private readonly StripeChargeService _stripeChargeService;

        public PaymentService(IOptions<PaymentSettings> paymentSettings, StripeChargeService stripeChargeService)
        {
            _paymentSettings = paymentSettings.Value;
            _stripeChargeService = stripeChargeService;
        }

        public bool ProcessPayment(string token, int amount)
        {
            var myCharge = new StripeChargeCreateOptions
            {
                Amount = amount,
                Currency = "usd",
                Description = "Bitcoin API Call",
                SourceTokenOrExistingSourceId = token,
                Capture = true
            };

            _stripeChargeService.ApiKey = _paymentSettings.StripePrivateKey;
            var stripeCharge = _stripeChargeService.Create(myCharge);

            var success = string.IsNullOrEmpty(stripeCharge.FailureCode) &&
                          string.IsNullOrEmpty(stripeCharge.FailureMessage);
            return success;
        }
    }
}
