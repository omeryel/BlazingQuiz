using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazingQuiz.Shared.DTOs
{
    public record AuthResponseDto(LoggedInUser user, string? ErrorMessage = null)
    {
        [JsonIgnore]
        public bool HasError => ErrorMessage != null;
    }
    
    
}
