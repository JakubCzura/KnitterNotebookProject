using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IThemeService : ICrudService<Theme>
    {
        Task<Theme?> GetByNameAsync(ApplicationTheme name);

        void ReplaceTheme(ApplicationTheme newThemeName, ApplicationTheme? oldThemeName = null);
    }
}