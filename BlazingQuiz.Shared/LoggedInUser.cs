using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazingQuiz.Shared
{
    public record LoggedInUser(int Id, string Name, string Role, string Token)
    {
        public string ToJson() => JsonSerializer.Serialize(this);

        public Claim[] ToClaims() => 
            [
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Name, Name),
                new Claim(nameof(Token), Token)
            ];

        public static LoggedInUser? LoadFrom(string json) => 
            string.IsNullOrWhiteSpace(json) ? null :
            JsonSerializer.Deserialize<LoggedInUser>(json); 
    }

}
