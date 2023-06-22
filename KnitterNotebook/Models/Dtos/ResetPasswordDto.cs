using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models.Dtos
{
    public class ResetPasswordDto
    {
        public string EmailOrNickname { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string RepeatedNewPassword { get; set; } = string.Empty;
    }
}
