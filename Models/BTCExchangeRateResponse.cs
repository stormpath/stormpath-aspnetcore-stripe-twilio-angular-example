using Newtonsoft.Json;

namespace stormpath_angularjs_dotnet_stripe_twilio.Models
{
    public class BtcExchangeRateResponse
    {
        public BtcExchangeRateCurrency Usd { get; set; }
    }

    public class BtcExchangeRateCurrency
    {
        [JsonProperty("24h")]
        public decimal? Last24H { get; set; }
    }
}
