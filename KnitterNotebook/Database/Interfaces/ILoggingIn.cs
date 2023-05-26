using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Interfaces
{
    public interface ILoggingIn
    {
        /// <summary>
        /// Log in user to application
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <param name="databaseContext">Database context</param>
        /// <returns>User instance if user is logged in, otherwise null</returns>
        Task<User>? LogInUser(string email, string password, DatabaseContext databaseContext);
    }
}