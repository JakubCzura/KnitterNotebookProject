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

namespace KnitterNotebook.Services;

public class ThemeService(DatabaseContext databaseContext) : CrudService<Theme>(databaseContext), IThemeService
{
    private readonly DatabaseContext _databaseContext = databaseContext;

    /// <summary>
    /// Checks if theme with given <paramref name="name"/> exists in database
    /// </summary>
    /// <param name="name">Theme's name</param>
    /// <returns>True if found, otherwise false</returns>
    public async Task<bool> ThemeExistsAsync(ApplicationTheme name) 
        => await _databaseContext.Themes.AsNoTracking()
                                        .AnyAsync(x => x.Name == name);  
    
    /// <summary>
    /// Returns theme's id with given <paramref name="name"/> if theme exists in database, otherwise null
    /// </summary>
    /// <param name="name">Theme's id</param>
    /// <returns>Theme's id if found, otherwise null</returns>
    public async Task<int?> GetThemeIdAsync(ApplicationTheme name) 
        => (await _databaseContext.Themes.AsNoTracking()
                                         .FirstOrDefaultAsync(x => x.Name == name))?.Id;

    /// <summary>
    /// Adds new theme's resource dictionary to merged dictionaries. Deletes old theme's resource dictionary if <paramref name="oldThemeName"/> is not null
    /// </summary>
    /// <param name="newThemeName">New theme to set</param>
    /// <param name="oldThemeName">Old theme to replace. Can be null when theme is set for the first time so there is not old theme to replace</param>
    /// <exception cref="InvalidEnumException"> when <paramref name="newThemeName"/> or <paramref name="oldThemeName"/> is not defined in <see cref="ApplicationTheme"/></exception>"
    public void ReplaceTheme(ApplicationTheme newThemeName, ApplicationTheme? oldThemeName = null)
    {
        if (!Enum.IsDefined(typeof(ApplicationTheme), newThemeName))
        {
            throw new InvalidEnumException(ExceptionsMessages.EnumInvalidValue(newThemeName));
        }
        if (oldThemeName is not null && !Enum.IsDefined(typeof(ApplicationTheme), oldThemeName))
        {
            throw new InvalidEnumException(ExceptionsMessages.EnumInvalidValue(oldThemeName));
        }

        string newThemeFullPath = Paths.Theme(newThemeName);

        //Get and delete current theme
        if (oldThemeName.HasValue)
        {
            string oldThemeFullPath = Paths.Theme(oldThemeName.Value);
            ResourceDictionary? themeDictionary = Application.Current?.Resources?.MergedDictionaries?.FirstOrDefault(x => x.Source.ToString().Equals(oldThemeFullPath, StringComparison.OrdinalIgnoreCase));
            Application.Current?.Resources?.MergedDictionaries?.Remove(themeDictionary);
        }

        //Set new theme
        Application.Current?.Resources?.MergedDictionaries?.Add(new ResourceDictionary { Source = new Uri(newThemeFullPath) });
    }
}