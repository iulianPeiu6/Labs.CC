using System.Text.Json.Serialization;
using UScheduler.WebApi.Users.Data.Entities;

namespace UScheduler.WebApi.Users.Models
{
    public class DisplayUserModel
    {
        public Guid Id { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Email { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime RegistrationDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AccountSettings AccountSettings { get; set; }
    }
}
