using System.Text.Json.Serialization;

namespace UScheduler.WebApi.Users.Models
{
    public class AuthenticateUserModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
