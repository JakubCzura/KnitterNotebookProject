using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.ViewModels.Services.Interfaces;
using System.Threading.Tasks;

namespace KnitterNotebook.ViewModels.Services
{
    public class MovieUrlService : IMovieUrlService
    {
        private readonly KnitterNotebookContext _knitterNotebookContext;

        public MovieUrlService(KnitterNotebookContext knitterNotebookContext)
        {
            _knitterNotebookContext = knitterNotebookContext;
        }

        public async Task DeleteMovieUrlAsync(MovieUrl movieUrl)
        {
            _knitterNotebookContext.Remove(movieUrl);
            await _knitterNotebookContext.SaveChangesAsync();
        }

        public async Task AddMovieUrlAsync(MovieUrl movieUrl)
        {
            await _knitterNotebookContext.AddAsync(movieUrl);
            await _knitterNotebookContext.SaveChangesAsync();
        }
    }
}