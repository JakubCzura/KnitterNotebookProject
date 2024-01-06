using KnitterNotebook.Database;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services;

/// <summary>
/// Generic service to perform CRUD operations in database
/// </summary>
/// <typeparam name="T">Entity to be stored in database</typeparam>
public class CrudService<T> : ICrudService<T> where T : BaseDbEntity
{
    private readonly DatabaseContext _databaseContext;
    private readonly DbSet<T> _dbSet;

    public CrudService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        _dbSet = _databaseContext.Set<T>();
    }

    /// <summary>
    /// Returns set of all <see cref="T"/> from database
    /// </summary>
    /// <returns>Set of all <see cref="T"/> from database</returns>
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

    /// <summary>
    /// Returns <see cref="T"/> with given <paramref name="id"/> from database
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <returns><see cref="T"/> with given <paramref name="id"/> from database if found, otherwise null</returns>
    public async Task<T?> GetAsync(int id) => await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    /// <summary>
    /// Adds <paramref name="entity"/> to database if it is not null
    /// </summary>
    /// <param name="entity">Entity that will be added to database</param>
    /// <returns>The number of state entries written to the database if <see cref="T"/> is not null, otherwise 0</returns>
    public async Task<int> CreateAsync(T entity)
    {
        if (entity is null)
        {
            return 0;
        }

        await _dbSet.AddAsync(entity);
        return await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    /// Updates <paramref name="entity"/> in database if it is not null
    /// </summary>
    /// <param name="entity">Entity that will be updated in database</param>
    /// <returns>The number of state entries written to the database if <see cref="T"/> is not null, otherwise 0</returns>
    public async Task<int> UpdateAsync(T entity)
    {
        if (entity is null)
        {
            return 0;
        }

        _dbSet.Update(entity);
        return await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes entity with given <paramref name="id"/> from database
    /// </summary>
    /// <param name="id">Id of entity</param>
    /// <returns>The total number of rows deleted in the database</returns>
    public async Task<int> DeleteAsync(int id) => await _dbSet.Where(x => x.Id == id).ExecuteDeleteAsync();
}