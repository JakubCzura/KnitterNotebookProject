using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Repositories;
using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class MovieUrlService : CrudService<MovieUrl>, IMovieUrlService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMovieUrlRepository _movieUrlRepository;
        public MovieUrlService(IMovieUrlRepository movieUrlRepository, IUserRepository userRepository) : base(movieUrlRepository)
        {
            _movieUrlRepository = movieUrlRepository;
            _userRepository = userRepository;
        }

        public async Task CreateAsync(CreateMovieUrl createMovieUrl)
        {
            MovieUrl movieUrl = new()
            {
                Title = createMovieUrl.Title,
                Link = new Uri(createMovieUrl.Link),
                User = await _userRepository.GetAsync(LoggedUserInformation.Id)
            };
            await _movieUrlRepository.CreateAsync(movieUrl);
        }
    }
}