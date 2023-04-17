using KnitterNotebook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Repositories.Interfaces
{
    public interface IThemeRepository
    {
        Task<List<Theme>> GetAllAsync();

        Task<Theme> GetAsync(int id);

        Task<Theme> GetByNameAsync(string name);
    }
}