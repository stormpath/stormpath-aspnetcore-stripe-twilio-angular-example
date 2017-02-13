using System;
using System.Threading.Tasks;
using Stormpath.SDK;
using Stormpath.SDK.Account;
using stormpath_angularjs_dotnet_stripe_twilio.Models;

namespace stormpath_angularjs_dotnet_stripe_twilio.Services
{
    public class AccountService
    {
        private readonly IAccount _account;
        public static readonly string BalanceKey = "Balance";
        public static readonly string TotalQueriesKey = "TotalQueries";

        public AccountService(IAccount account)
        {
            _account = account;
        }
        
        public async Task<UserAccountInfo> GetUserAccountInfo()
        {
            var userAccountInfo = new UserAccountInfo();
            var apiKeys = await _account.GetApiKeys().FirstAsync();
            var accountCustomData = await _account.GetCustomDataAsync();

            userAccountInfo.ApiKeyId = apiKeys.Id;
            userAccountInfo.ApiKeySecret = apiKeys.Secret;
            userAccountInfo.Balance = decimal.Parse(accountCustomData[BalanceKey].ToString());
            userAccountInfo.TotalQueries = int.Parse(accountCustomData[TotalQueriesKey].ToString());

            return userAccountInfo;
        }

        public async Task UpdateUserBalance(decimal amount)
        {
            var customData = await _account.GetCustomDataAsync();
            var oldValue = decimal.Parse(customData[BalanceKey].ToString());

            customData[BalanceKey] = oldValue + amount;
            await customData.SaveAsync();
        }

        public async Task UpdateUserTotalQueries(int totalQueries)
        {
            var customData = await _account.GetCustomDataAsync();
            var oldValue = int.Parse(customData[TotalQueriesKey].ToString());

            customData[TotalQueriesKey] = oldValue + totalQueries;
            await customData.SaveAsync();
        }
    }
}
