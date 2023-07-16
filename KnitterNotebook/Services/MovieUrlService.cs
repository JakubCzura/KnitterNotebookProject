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
        private readonly DatabaseContext _databaseContext;
        private readonly IUserService _userService;

        public MovieUrlService(DatabaseContext databaseContext, IUserService userService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _userService = userService;
        }

        public async Task CreateAsync(CreateMovieUrlDto createMovieUrl)
        {
            MovieUrl movieUrl = new()
            {
                Title = createMovieUrl.Title,
                Link = new Uri(createMovieUrl.Link),
                User = await _userService.GetAsync(LoggedUserInformation.Id),
            };
            await _databaseContext.AddAsync(movieUrl);
            await _databaseContext.SaveChangesAsync();
        }
    }
}