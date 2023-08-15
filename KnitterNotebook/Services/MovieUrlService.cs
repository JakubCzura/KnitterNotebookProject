using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class MovieUrlService : CrudService<MovieUrl>, IMovieUrlService
    {
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
                UserId = createMovieUrl.UserId,
            };
            await _databaseContext.AddAsync(movieUrl);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<MovieUrl>> GetUserMovieUrlsAsync(int userId)
            => await _databaseContext.MovieUrls.Where(x => x.UserId == userId).ToListAsync();
    }
}