using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services;

public class MovieUrlService : CrudService<MovieUrl>, IMovieUrlService
{
    private readonly DatabaseContext _databaseContext;

    public MovieUrlService(DatabaseContext databaseContext) : base(databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <summary>
    /// Adds new movie url to database
    /// </summary>
    /// <param name="createMovieUrl">Dto model that should be added to database as movie url</param>
    /// <returns>1 if object was added to database</returns>
    /// <exception cref="NullReferenceException"></exception>"
    /// <exception cref="UriFormatException"></exception>"
    public async Task<int> CreateAsync(CreateMovieUrlDto createMovieUrl)
    {
        MovieUrl movieUrl = new()
        {
            Title = createMovieUrl.Title,
            Link = new Uri(createMovieUrl.Link),
            Description = createMovieUrl.Description,
            UserId = createMovieUrl.UserId,
        };
        await _databaseContext.AddAsync(movieUrl);
        return await _databaseContext.SaveChangesAsync();
    }

    public async Task<List<MovieUrlDto>> GetUserMovieUrlsAsync(int userId)
        => await _databaseContext.MovieUrls.Where(x => x.UserId == userId)
                                           .Select(x => new MovieUrlDto(x)).ToListAsync();
}