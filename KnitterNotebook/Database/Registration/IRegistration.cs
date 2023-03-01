using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Registration
{
    public interface IRegistration
    {
        /// <summary>
        /// Registers user
        /// </summary>
        /// <param name="user">User to register</param>
        /// <param name="knitterNotebookContext">Instance of database context</param>
        /// <returns></returns>
        Task RegisterUser(User user, KnitterNotebookContext knitterNotebookContext);
    }
}