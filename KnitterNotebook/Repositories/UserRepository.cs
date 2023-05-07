using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Repositories.Interfaces;

namespace KnitterNotebook.Repositories
{
    public class UserRepository : CrudRepository<User>, IUserRepository
    {
        public UserRepository(KnitterNotebookContext knitterNotebookContext) : base(knitterNotebookContext)
        {
        }
    }
}