using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class MovieUrlService : CrudService<MovieUrl>, IMovieUrlService
    {
        public MovieUrlService(IMovieUrlRepository movieUrlRepository) : base(movieUrlRepository)
        {
        }
    }
}