using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Interfaces
{
    public interface IAddingMovieUrl
    {
        /// <summary>
        /// Adds movie to database
        /// </summary>
        /// <param name="movieUrl">Movie to add</param>
        /// <param name="knitterNotebookContext">Instance of database context</param>
        /// <returns>Task of adding movie to database</returns>
        public Task AddMovieUrl(MovieUrl movieUrl, KnitterNotebookContext knitterNotebookContext);
    }
}