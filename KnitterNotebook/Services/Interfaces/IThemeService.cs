using KnitterNotebook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IThemeService
    {
        Task<List<Theme>> GetAllAsync();

        Task<Theme> GetAsync(int id);

        Task<Theme> GetByNameAsync(string name);
    }
}