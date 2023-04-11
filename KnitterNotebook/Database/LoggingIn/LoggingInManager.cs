using KnitterNotebook.Database.Interfaces;
using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Login
{
    public class LoggingInManager
    {
        private readonly ILoggingIn _loggingIn;
        private readonly string _email;
        private readonly string _password;
        private readonly KnitterNotebookContext _knitterNotebookContext;

        public LoggingInManager(ILoggingIn loggingIN, string email, string password, KnitterNotebookContext knitterNotebookContext)
        {
            _loggingIn = loggingIN;
            _email = email;
            _password = password;
            _knitterNotebookContext = knitterNotebookContext;
        }

        /// <summary>
        /// Log in user to application
        /// </summary>
        /// <returns>User instance if user is logged in, otherwise null</returns>
        public async Task<User>? LogIn()
        {
            return await _loggingIn.LogInUser(_email, _password, _knitterNotebookContext)!;
        }
    }
}