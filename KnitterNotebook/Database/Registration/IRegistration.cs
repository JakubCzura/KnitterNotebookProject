using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Registration
{
    public interface IRegistration
    {
        Task RegisterUser(User user, KnitterNotebookContext knitterNotebookContext);
    }
}