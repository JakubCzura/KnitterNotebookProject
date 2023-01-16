using KnitterNotebook.Models;
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

        /// <summary>
        /// Log in user to application
        /// </summary>
        /// <returns>User instance if user is logged in, otherwise null</returns>
        public async Task<User>? LogIn()
        {
            return await LoggingIn.LogInUser(Email, Password, KnitterNotebookContext);
        }
    }
}