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

public class MovieUrlService(DatabaseContext databaseContext) : CrudService<MovieUrl>(databaseContext), IMovieUrlService
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    /// <summary>
    /// Adds new movie url to database
    /// </summary>
    /// <param name="createMovieUrl">Dto model that should be added to database as movie url</param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="NullReferenceException">If <paramref name="createMovieUrl"/> is null</exception>"
    /// <exception cref="UriFormatException">If <paramref name="createMovieUrl.Link"/> is not in proper format</exception>"
    public async Task<int> CreateAsync(CreateMovieUrlDto createMovieUrl)
    {
        MovieUrl movieUrl = new(createMovieUrl);
        await _databaseContext.AddAsync(movieUrl);
        return await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    /// Returns all movies urls for user with given <paramref name="userId"/> from database
    /// </summary>
    /// <param name="userId">User's id</param>
    /// <returns>List of user's all movie urls</returns>
    public async Task<List<MovieUrlDto>> GetUserMovieUrlsAsync(int userId)
        => await _databaseContext.MovieUrls.AsNoTracking()
                                           .Where(x => x.UserId == userId)
                                           .Select(x => new MovieUrlDto(x))
                                           .ToListAsync();
}