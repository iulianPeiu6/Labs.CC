using System.Text.Json.Serialization;
using UScheduler.WebApi.Users.Data.Entities;

namespace UScheduler.WebApi.Users.Models
{
    public class DisplayUserModel
    {
        public Guid Id { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public DateTime RegistrationDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AccountSettings? AccountSettings { get; set; }
    }
}
