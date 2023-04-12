using KnitterNotebook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Registration
{
    public class UserExistence
    {
        /// <summary>
        /// Checks if user already exists in database
        /// </summary>
        /// <param name="user">User to check</param>
        /// <param name="knitterNotebookContext">Instance of database context</param>
        /// <returns></returns>
        public static async Task<bool> IfUserAlreadyExists(User user, KnitterNotebookContext knitterNotebookContext)
        {
            if (await knitterNotebookContext.Users.AnyAsync(x => x.Nickname == user.Nickname || x.Email == user.Email))
            {
                throw new Exception($"Użytkownik z nickiem {user.Nickname} już istnieje");
            }
            return true;
        }
    }
}
