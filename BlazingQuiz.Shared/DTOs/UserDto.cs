using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingQuiz.Shared.DTOs
{
    //public record UserDto(int Id, string Name, string Email, string Phone, bool IsApproved);

    public class UserDto
    {
        public UserDto(int id, string name, string email, string phone, bool isApproved)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            IsApproved = isApproved;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsApproved { get; set; }
    }




}
