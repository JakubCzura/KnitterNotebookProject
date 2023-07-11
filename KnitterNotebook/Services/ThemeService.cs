using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class ThemeService : CrudService<Theme>, IThemeService
    {
        private readonly DatabaseContext _databaseContext;

        public ThemeService(DatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Theme?> GetByNameAsync(string name)
            => await _databaseContext.Themes.FirstOrDefaultAsync(x => x.Name == name);
        
    }
}