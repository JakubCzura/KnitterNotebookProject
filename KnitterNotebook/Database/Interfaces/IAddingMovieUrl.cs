using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Interfaces
{
    public interface IAddingMovieUrl
    {
        public Task AddMovieUrl(MovieUrl movieUrl, KnitterNotebookContext knitterNotebookContext);
    }
}