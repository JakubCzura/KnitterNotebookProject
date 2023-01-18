using KnitterNotebook.ViewModels;
using System.Windows;

namespace KnitterNotebook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App Instance { get; private set; } = null!;

        public App()
        {
            Instance = this;
        }
    }
}