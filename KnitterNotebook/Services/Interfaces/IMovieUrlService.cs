using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IMovieUrlService : ICrudService<MovieUrl>
    {
        Task CreateAsync(CreateMovieUrlDto data);

        Task<List<MovieUrl>> GetUserMovieUrlsAsync(int userId);
    }
}