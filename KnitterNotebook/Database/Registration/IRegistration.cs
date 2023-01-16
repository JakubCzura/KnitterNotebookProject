using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Registration
{
    public interface IRegistration
    {
        Task<bool> RegisterUser(User user, KnitterNotebookContext knitterNotebookContext);
    }
}