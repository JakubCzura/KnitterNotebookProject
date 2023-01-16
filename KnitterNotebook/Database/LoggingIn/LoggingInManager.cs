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
        private ILoggingIn LoggingIn { get; set; } 
        private string Email { get; set; } 
        private string Password { get; set; } 

        private KnitterNotebookContext KnitterNotebookContext { get; set; }

        public LoggingInManager(ILoggingIn loggingIN, string email, string password, KnitterNotebookContext knitterNotebookContext)
        {
            LoggingIn = loggingIN;
            Email = email;
            Password = password;
            KnitterNotebookContext = knitterNotebookContext;
        }

        public async Task<User>? LogIn()
        {
            return await LoggingIn.LogInUser(Email, Password, KnitterNotebookContext);
        }
    }
}
