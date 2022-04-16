namespace UScheduler.WebApi.Users.Data.Entities
{
    public class AccountSettings
    {
        public Guid Id { get; set; }

        public bool SendNotificationOnEmail { get; set; }

        public string EmailForNotification { get; set; }
    }
}