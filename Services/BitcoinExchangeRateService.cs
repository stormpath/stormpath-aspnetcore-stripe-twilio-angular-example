using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using stormpath_angularjs_dotnet_stripe_twilio.Models;

namespace stormpath_angularjs_dotnet_stripe_twilio.Services
{
    public class BitcoinExchangeRateService
    {
        public async Task<decimal> GetBitcoinExchangeRate()
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync("http://api.bitcoincharts.com/v1/weighted_prices.json"))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Failed to retrieve BTC exchange rates");
                }

                var responseBody = JsonConvert.DeserializeObject<BtcExchangeRateResponse>(await response.Content.ReadAsStringAsync());

                if (responseBody.Usd?.Last24H == null)
                {
                    throw new Exception("Failed to retrieve BTC exchange rates");
                }

                return responseBody.Usd.Last24H.Value;
            }
        }
    }
}
