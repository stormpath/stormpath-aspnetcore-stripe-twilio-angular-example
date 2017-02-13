using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using stormpath_angularjs_dotnet_stripe_twilio.Models;

namespace stormpath_angularjs_dotnet_stripe_twilio.Services
{
    public class SmsService
    {
        private readonly SmsSettings _smsSettings;

        public SmsService(IOptions<SmsSettings> smsSettings)
        {
            _smsSettings = smsSettings.Value;
        }

        public async Task SendSms(string message, string phoneNumber)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(_smsSettings.BaseUri) })
            {
                phoneNumber = phoneNumber.Trim();

                if (phoneNumber.StartsWith("+"))
                {
                    phoneNumber = phoneNumber.Substring(1);
                }

                var basicHeaderValue = $"{_smsSettings.Sid}:{_smsSettings.Token}";
                client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(basicHeaderValue)));

                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["To"] = $"+{phoneNumber}",
                    ["From"] = _smsSettings.From,
                    ["Body"] = message
                });

                var response = await client.PostAsync(_smsSettings.RequestUri, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("An error occurred while sending the SMS");
                }
            }
       }
    }
}
