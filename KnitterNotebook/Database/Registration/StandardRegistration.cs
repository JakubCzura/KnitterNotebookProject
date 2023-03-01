using KnitterNotebook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Registration
{
    public class StandardRegistration : IRegistration
    {
        public async Task RegisterUser(User user, KnitterNotebookContext knitterNotebookContext)
        {
            if (await IfUserAlreadyExists(user, knitterNotebookContext))
            {
                throw new Exception($"User with name {user.Nickname} already exists");
            }
            else
            {
                await knitterNotebookContext.AddAsync(user);
                await knitterNotebookContext.SaveChangesAsync();
            }
        }

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
                return true;
            }
            return false;
        }
    }
}