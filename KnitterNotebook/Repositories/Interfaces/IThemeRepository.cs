using KnitterNotebook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Repositories.Interfaces
{
    public interface IThemeRepository : ICrudRepository<Theme>
    {
        Task<Theme> GetByNameAsync(string name);
    }
}