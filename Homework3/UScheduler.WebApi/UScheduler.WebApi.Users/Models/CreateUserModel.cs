namespace UScheduler.WebApi.Users.Models
{
    public class CreateUserModel
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? HashedPassword { get; set; }
    }
}
