using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Login
{
    public interface ILoggingIn
    {
        Task<User>? LoginUser(string nickname, string password, KnitterNotebookContext knitterNotebookContext);
    }
}
