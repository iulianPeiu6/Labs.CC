namespace SendGrid.Options
{
    public class SendGridConfiguration
    {
        public string ApiKey { get; set; }
        public string WebApiEndpoint { get; set; }
        public string SenderEmailAddress { get; set; }
        public string BaseSubject { get; set; }
    }
}