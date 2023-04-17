using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Repositories
{
    public class ThemeRepository : IThemeRepository
    {
        private readonly KnitterNotebookContext _knitterNotebookContext;

        public ThemeRepository(KnitterNotebookContext knitterNotebookContext)
        {
            _knitterNotebookContext = knitterNotebookContext;
        }

        public async Task<List<Theme>> GetAllAsync()
        {
            return await _knitterNotebookContext.Themes.ToListAsync();
        }

        public async Task<Theme> GetAsync(int id)
        {
            return await _knitterNotebookContext.Themes.FindAsync(id) ?? null!;
        }

        public async Task<Theme> GetByNameAsync(string name)
        {
            return await _knitterNotebookContext.Themes.FirstOrDefaultAsync(x => x.Name == name) ?? null!;
        }
    }
}