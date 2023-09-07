using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IMovieUrlService : ICrudService<MovieUrl>
    {
        Task<int> CreateAsync(CreateMovieUrlDto data);

        Task<List<MovieUrlDto>> GetUserMovieUrlsAsync(int userId);
    }
}