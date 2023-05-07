using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
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
        private readonly ICrudRepository<T> _crudRepository;

        public CrudService(ICrudRepository<T> crudRepository)
        {
            _crudRepository = crudRepository;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _crudRepository.GetAllAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _crudRepository.GetAsync(id);
        }

        public async Task CreateAsync(T entity)
        {
            await _crudRepository.CreateAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _crudRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _crudRepository.DeleteAsync(id);
        }
    }
}