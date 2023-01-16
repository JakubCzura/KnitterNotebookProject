using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Login
{
    public interface ILoggingIn
    {
        Task<User>? LogInUser(string email, string password, KnitterNotebookContext knitterNotebookContext);
    }
}