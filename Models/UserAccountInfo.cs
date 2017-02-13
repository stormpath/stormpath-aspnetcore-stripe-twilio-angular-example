namespace stormpath_angularjs_dotnet_stripe_twilio.Models
{
    public class UserAccountInfo
    {
        public string ApiKeyId { get; set; }
        public string ApiKeySecret { get; set; }
        public decimal Balance { get; set; }
        public int TotalQueries { get; set; }
    }
}
