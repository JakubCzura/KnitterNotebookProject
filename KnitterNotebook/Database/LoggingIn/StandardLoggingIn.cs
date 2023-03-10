using KnitterNotebook.Database.Interfaces;
using KnitterNotebook.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Login
{
    public class StandardLoggingIn : ILoggingIn
    {
        public async Task<User>? LogInUser(string email, string password, KnitterNotebookContext knitterNotebookContext)
        {
            User? user = await knitterNotebookContext.Users
                //  .Where(x => x.Email == email)
                // .Include(x => x.Theme)
                //  .Include(x => x.Projects)
                //  .Include(x => x.MovieUrls)
                .FirstOrDefaultAsync(x => x.Email == email);
            if (user != null)
            {
                if (PasswordHasher.VerifyPassword(password, user.Password))
                {
                    return user;
                }
                return null!;
            }
            return null!;
        }
    }
}