using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Repositories.Interfaces
{
    /// <summary>
    /// Generic interface to perform database operations
    /// </summary>
    /// <typeparam name="T">Object to be stored in database</typeparam>
    public interface ICrudRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetAsync(int id);

        Task CreateAsync(T data);

        Task UpdateAsync(T data);

        Task DeleteAsync(int id);
    }
}