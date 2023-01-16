using KnitterNotebook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Login
{
    public class StandardLoggingIn : ILoggingIn
    {
        public async Task<User>? LoginUser(string nickname, string password, KnitterNotebookContext knitterNotebookContext)
        {
            return await knitterNotebookContext.Users.FirstOrDefaultAsync(x => x.Nickname == nickname && PasswordHasher.VerifyPassword(password, x.Password));
        }
    }
}
