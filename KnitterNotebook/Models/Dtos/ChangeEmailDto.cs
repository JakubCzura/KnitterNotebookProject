using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models.Dtos
{
    public class ChangeEmailDto
    {
        public ChangeEmailDto(int userId, string email)
        {
            UserId = userId;
            Email = email;
        }

        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;
    }
}
