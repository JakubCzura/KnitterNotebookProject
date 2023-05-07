using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IThemeService : ICrudService<Theme>
    {
        Task<Theme> GetByNameAsync(string name);
    }
}