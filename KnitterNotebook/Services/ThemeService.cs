using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.Services
{
    public class ThemeService : CrudService<Theme>, IThemeService
    {
        private readonly DatabaseContext _databaseContext;

        public ThemeService(DatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <returns>Theme object if theme with given name was found otherwise null</returns>
        public async Task<Theme?> GetByNameAsync(string name) => await _databaseContext.Themes.FirstOrDefaultAsync(x => x.Name == name);

        /// <summary>
        /// Adds new theme's resource dictionary to merged dictionaries and deletes old theme's resource dictionary if oldResourceDictionaryFullPath is not null
        /// </summary>
        /// <param name="newResourceDictionaryFullPath">Full path with extension of new theme's resource dictionary</param>
        /// <param name="oldResourceDictionaryFullPath">Full path with extension of old theme's resource dictionary or null if there is no need to delete to delete the dictionary</param>
        public void ReplaceTheme(string newResourceDictionaryFullPath, string? oldResourceDictionaryFullPath = null)
        {
            //Get and delete current theme
            if (oldResourceDictionaryFullPath != null)
            {
                ResourceDictionary? result = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source.ToString().Equals(oldResourceDictionaryFullPath, StringComparison.OrdinalIgnoreCase));
                Application.Current.Resources.MergedDictionaries.Remove(result);
            }

            //Set new theme
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(newResourceDictionaryFullPath) });
        }
    }
}