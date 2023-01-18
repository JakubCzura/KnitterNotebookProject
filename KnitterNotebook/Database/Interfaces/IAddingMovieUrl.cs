using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Interfaces
{
    internal interface IAddingMovieUrl
    {
        public Task AddMovieUrl(MovieUrl movieUrl, KnitterNotebookContext knitterNotebookContext);
    }
}