using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Registration
{
    public interface IRegistration
    {
        Task<bool> RegisterUser(User user, KnitterNotebookContext knitterNotebookContext);
    }
}
