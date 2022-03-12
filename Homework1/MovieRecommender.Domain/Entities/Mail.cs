namespace MovieRecommender.Domain.Entities
{
    public class Mail
    {
        public string SenderEmailAddress { get; set; }
        public string ReceiverEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
