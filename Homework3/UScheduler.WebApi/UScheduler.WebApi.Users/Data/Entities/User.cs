namespace UScheduler.WebApi.Users.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string HashedPassword { get; set; }

        public Guid AccountSettingsId { get; set; }

        public AccountSettings AccountSettings { get; set; }
    }
}
