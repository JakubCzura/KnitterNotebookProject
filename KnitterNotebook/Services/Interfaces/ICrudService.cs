using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces;

/// <summary>
/// Generic interface to perform crud operations in database
/// </summary>
/// <typeparam name="T">Objects to be stored in database</typeparam>
public interface ICrudService<T> where T : BaseDbEntity
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetAsync(int id);

    Task<int> CreateAsync(T data);

    Task<int> UpdateAsync(T data);

    Task<int> DeleteAsync(int id);
}