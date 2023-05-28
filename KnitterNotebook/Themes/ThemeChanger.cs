using System;
using System.Linq;
using System.Windows;

namespace KnitterNotebook.Themes
{
    public class ThemeChanger
    {
        /// <summary>
        /// Sets current theme
        /// </summary>
        /// <param name="resourceDictionaryFullName">Full path of resource dictionary of selected theme, like c:\..\DefaultMode.xaml</param>
        public static void SetTheme(string resourceDictionaryFullName)
        {
            if (!string.IsNullOrWhiteSpace(resourceDictionaryFullName))
            {
                //Gets current theme
                var result = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source.ToString().Contains("Mode.xaml"));
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(resourceDictionaryFullName) });
            }
        }
    }
}