using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KnitterNotebook.Repositories
{
    public class ThemeRepository : CrudRepository<Theme>, IThemeRepository
    {
        private readonly KnitterNotebookContext _knitterNotebookContext;

        public ThemeRepository(KnitterNotebookContext knitterNotebookContext) : base(knitterNotebookContext)
        {
            _knitterNotebookContext = knitterNotebookContext;
        }

        public async Task<Theme> GetByNameAsync(string name)
        {
            return await _knitterNotebookContext.Themes.FirstOrDefaultAsync(x => x.Name == name) ?? null!;
        }
    }
}