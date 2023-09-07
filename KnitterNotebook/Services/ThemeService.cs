using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
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

        public async Task<bool> ThemeExistsAsync(ApplicationTheme name) => await _databaseContext.Themes.AnyAsync(x => x.Name == name);

        /// <returns>Theme object if theme with given name was found otherwise null</returns>
        public async Task<int?> GetThemeIdAsync(ApplicationTheme name) => (await _databaseContext.Themes.FirstOrDefaultAsync(x => x.Name == name))?.Id;

        /// <summary>
        /// Adds new theme's resource dictionary to merged dictionaries and deletes old theme's resource dictionary if oldResourceDictionaryFullPath is not null
        /// </summary>
        /// <param name="newThemeName">Full path with extension of new theme's resource dictionary</param>
        /// <param name="oldThemeName">Full path with extension of old theme's resource dictionary or null if there is no need to delete to delete the dictionary</param>
        public void ReplaceTheme(ApplicationTheme newThemeName, ApplicationTheme? oldThemeName = null)
        {
            string newThemeFullPath = Paths.ThemeFullPath(newThemeName);

            //Get and delete current theme
            if (oldThemeName.HasValue)
            {
                string oldThemeFullPath = Paths.ThemeFullPath(oldThemeName.Value);
                ResourceDictionary? result = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source.ToString().Equals(oldThemeFullPath, StringComparison.OrdinalIgnoreCase));
                Application.Current.Resources.MergedDictionaries.Remove(result);
            }

            //Set new theme
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(newThemeFullPath) });
        }
    }
}