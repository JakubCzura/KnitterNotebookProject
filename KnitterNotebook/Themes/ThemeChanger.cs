using KnitterNotebook.ViewModels;

namespace KnitterNotebook.Themes
{
    public class ThemeChanger : BaseViewModel
    {
        //private static readonly string DataPath = Path.Combine(Environment.CurrentDirectory, "ThemesData.txt");
        ////private static readonly string DataPath = new Uri($"/AppThemes/ThemesData.txt", UriKind.Relative).ToString();

        //public static void SaveTheme(string theme)
        //{
        //    try
        //    {
        //        using (StreamWriter StreamWriter = new StreamWriter(DataPath, false))
        //        {
        //            StreamWriter.WriteLine(theme.ToString());
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show(exception.Message);
        //    }
        //}

        //public static string ReadTheme()
        //{
        //    try
        //    {
        //        if (File.Exists(DataPath))
        //        {
        //            using (StreamReader StreamReader = new StreamReader(DataPath))
        //            {
        //                return StreamReader.ReadLine();
        //            }
        //        }
        //        else
        //        {
        //            return AppThemes.Standard.ToString();
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show(exception.Message);
        //        return AppThemes.Standard.ToString();
        //    }
        //}

        //public static void SetTheme()
        //{
        //    try
        //    {
        //        App.Current.Resources.Clear();
        //        App.Current.Resources.MergedDictionaries.Clear();
        //        //App.Current.Resources.Source = new Uri($"/AppThemes/{ReadTheme()}.xaml", UriKind.Relative);

        //        App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri($"/AppThemes/CommonTheme.xaml", UriKind.Relative) });
        //        App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri($"/AppThemes/{ReadTheme()}.xaml", UriKind.Relative) });
        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show(exception.Message);
        //    }
        //}
    }
}