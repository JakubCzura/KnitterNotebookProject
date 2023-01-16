using KnitterNotebook.Database.Registration;
using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Login
{
    public class LoggingInManager
    {
        private ILoggingIn Login { get; set; }
        private string Nickname { get; set; }
        private string Password { get; set; }

        private KnitterNotebookContext KnitterNotebookContext { get; set; }

        public LoggingInManager(ILoggingIn login, string nickname, string password, KnitterNotebookContext knitterNotebookContext)
        {
            Login = login;
            Nickname = nickname;
            Password = password;
            KnitterNotebookContext = knitterNotebookContext;
        }

        public async Task<User>? LoginUser()
        {
            return await Login.LoginUser(Nickname, Password, KnitterNotebookContext);
        }
    }
}
