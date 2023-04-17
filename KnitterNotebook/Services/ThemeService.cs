using KnitterNotebook.Models;
using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class ThemeService : IThemeService
    {
        private readonly IThemeRepository _themeRepository;

        public ThemeService(IThemeRepository themeRepository)
        {
            _themeRepository = themeRepository;
        }

        public async Task<List<Theme>> GetAllAsync()
        {
            return await _themeRepository.GetAllAsync();
        }

        public async Task<Theme> GetAsync(int id)
        {
            return await _themeRepository.GetAsync(id);
        }

        public async Task<Theme> GetByNameAsync(string name)
        {
            return await _themeRepository.GetByNameAsync(name);
        }
    }
}