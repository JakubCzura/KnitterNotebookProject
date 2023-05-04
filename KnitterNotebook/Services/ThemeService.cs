using KnitterNotebook.Models;
using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class ThemeService : CrudService<Theme>, IThemeService
    {
        private readonly IThemeRepository _themeRepository;

        public ThemeService(IThemeRepository themeRepository) : base(themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public async Task<Theme> GetByNameAsync(string name)
        {
            return await _themeRepository.GetByNameAsync(name);
        }
    }
}