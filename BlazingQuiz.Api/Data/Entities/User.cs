using BlazingQuiz.Shared;
using System.ComponentModel.DataAnnotations;

namespace BlazingQuiz.Api.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(150)]
        public string Email { get; set; }
        [Length(10, 15)]
        public string Phone { get; set; }
        [MaxLength(250)]
        public string PasswordHash { get; set; }
        public bool IsApproved { get; set; }

        [MaxLength(15)]
        public string Role { get; set; } = nameof(UserRole.Student);

    }
}
