using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Registration
{
    public class StandardRegistration : IRegistration
    {
        public async Task RegisterUser(User user, KnitterNotebookContext knitterNotebookContext)
        {
            await knitterNotebookContext.AddAsync(user);
            await knitterNotebookContext.SaveChangesAsync();
        }
    }
}