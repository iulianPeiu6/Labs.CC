namespace UScheduler.WebApi.Users.Options
{
    public class AuthenticationConfiguration
    {
        public string Issuer { get; set; }

        public string Key { get; set; }

        public int MinutesExpiration { get; set; }
    }
}
