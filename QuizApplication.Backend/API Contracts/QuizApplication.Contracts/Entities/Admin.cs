using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QuizApplication.Contracts.Entities
{
    public class Admin : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
