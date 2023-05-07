using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IMovieUrlService : ICrudService<MovieUrl>
    {
        Task CreateAsync(CreateMovieUrl data);
    }
}