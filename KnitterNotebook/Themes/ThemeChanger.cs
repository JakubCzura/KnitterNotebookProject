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
        /// <param name="resourceDictionaryFullName">Full path of resource dictionary of selected theme, like DefaultMode.xaml</param>
        public static void SetTheme(string resourceDictionaryFullName)
        {
            try
            {
               // App.Current.Resources.Clear();
                //App.Current.Resources.MergedDictionaries.Clear();
                //App.Current.Resources.Source = new Uri($"/AppThemes/{ReadTheme()}.xaml", UriKind.Relative);
                
                //Gets current theme
                var result = App.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source.ToString().Contains("Mode.xaml"));
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(resourceDictionaryFullName) });
               
                MessageBox.Show(result.ToString());
               // App.Current.Resources.MergedDictionaries.Remove(DarkMode.xaml);
               // App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri($"/AppThemes/{ReadTheme()}.xaml", UriKind.Relative) });
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}