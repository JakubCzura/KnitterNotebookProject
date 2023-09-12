using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Exceptions.Messages;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Xunit.Sdk;

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
        /// <param name="newThemeName">New theme to set</param>
        /// <param name="oldThemeName">Old theme to replace. Can be null when theme is set for the first time so there is not old theme to replace</param>
        public void ReplaceTheme(ApplicationTheme newThemeName, ApplicationTheme? oldThemeName = null)
        {
            if(!Enum.IsDefined(typeof(ApplicationTheme), newThemeName))
            {
                throw new InvalidEnumException(ExceptionsMessages.EnumInvalidValue(newThemeName));
            }
            if (oldThemeName is not null && !Enum.IsDefined(typeof(ApplicationTheme), oldThemeName))
            {
                throw new InvalidEnumException(ExceptionsMessages.EnumInvalidValue(oldThemeName));
            }

            string newThemeFullPath = Paths.ThemeFullPath(newThemeName);

            //Get and delete current theme
            if (oldThemeName.HasValue)
            {
                string oldThemeFullPath = Paths.ThemeFullPath(oldThemeName.Value);
                ResourceDictionary? result = Application.Current?.Resources?.MergedDictionaries?.FirstOrDefault(x => x.Source.ToString().Equals(oldThemeFullPath, StringComparison.OrdinalIgnoreCase));
                Application.Current?.Resources?.MergedDictionaries?.Remove(result);
            }

            //Set new theme
            Application.Current?.Resources?.MergedDictionaries?.Add(new ResourceDictionary { Source = new Uri(newThemeFullPath) });
        }
    }
}