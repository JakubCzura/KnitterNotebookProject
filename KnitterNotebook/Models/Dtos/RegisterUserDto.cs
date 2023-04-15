using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models.Dtos
{
    public class RegisterUserDto
    {
        public RegisterUserDto(string nickname, string email, string password)
        {
            Nickname = nickname;
            Email = email;
            Password = password;
        }

        public string Nickname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
