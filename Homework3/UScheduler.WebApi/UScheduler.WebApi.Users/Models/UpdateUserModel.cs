using UScheduler.WebApi.Users.Data.Entities;

namespace UScheduler.WebApi.Users.Models
{
    public class UpdateUserModel
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? HashedPassword { get; set; }

        public AccountSettings? AccountSettings { get; set; }
    }
}
