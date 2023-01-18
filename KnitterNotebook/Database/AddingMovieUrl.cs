using KnitterNotebook.Database.Interfaces;
using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database
{
    public class AddingMovieUrl : IAddingMovieUrl
    {
        public async Task AddMovieUrl(MovieUrl movieUrl, KnitterNotebookContext knitterNotebookContext)
        {
            await knitterNotebookContext.AddAsync(movieUrl);
            await knitterNotebookContext.SaveChangesAsync();
        }
    }
}