using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class MovieUrlService : CrudService<MovieUrl>, IMovieUrlService
    {
        //private readonly IUserRepository _userRepository;
        //private readonly IMovieUrlRepository _movieUrlRepository;
        private readonly DatabaseContext _databaseContext;

        public MovieUrlService(DatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task CreateAsync(CreateMovieUrlDto createMovieUrl)
        {
            MovieUrl movieUrl = new()
            {
                Title = createMovieUrl.Title,
                Link = new Uri(createMovieUrl.Link),
                User = createMovieUrl.User
            };
            await _databaseContext.AddAsync(movieUrl);
            await _databaseContext.SaveChangesAsync();
        }
    }
}