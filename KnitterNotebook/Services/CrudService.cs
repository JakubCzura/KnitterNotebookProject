using KnitterNotebook.Database;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    /// <summary>
    /// Generic class to perform crud operations in database
    /// </summary>
    /// <typeparam name="T">Object to be stored in database</typeparam>
    public class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly DatabaseContext _databaseContext;
        private readonly DbSet<T> _dbSet;

        public CrudService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _dbSet = _databaseContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T> GetAsync(int id)
            => await _dbSet.FindAsync(id) ?? null!;

        public async Task CreateAsync(T data)
        {
            await _dbSet.AddAsync(data);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T data)
        {
            _dbSet.Update(data);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T data = await _dbSet.FindAsync(id);
            _dbSet.Remove(data);
            await _databaseContext.SaveChangesAsync();
        }
    }
}