using KnitterNotebook.ViewModels;
using System;
using System.IO;
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
            try
            {
                //Gets current theme
                var result = App.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source.ToString().Contains("Mode.xaml"));
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(resourceDictionaryFullName) });
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}