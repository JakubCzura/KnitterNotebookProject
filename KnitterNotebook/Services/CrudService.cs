using KnitterNotebook.Database;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services;

/// <summary>
/// Generic class to perform crud operations in database
/// </summary>
/// <typeparam name="T">Object to be stored in database</typeparam>
public class CrudService<T> : ICrudService<T> where T : BaseDbEntity
{
    private readonly DatabaseContext _databaseContext;
    private readonly DbSet<T> _dbSet;

    public CrudService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        _dbSet = _databaseContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

    public async Task<T?> GetAsync(int id) => await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<int> CreateAsync(T data)
    {
        if (data is null) return 0;

        await _dbSet.AddAsync(data);
        return await _databaseContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(T data)
    {
        if (data is null) return 0;

        _dbSet.Update(data);
        return await _databaseContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id) => await _dbSet.Where(x => x.Id == id).ExecuteDeleteAsync();
}