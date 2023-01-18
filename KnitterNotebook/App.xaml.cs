using KnitterNotebook.ViewModels;
using System.Windows;

namespace KnitterNotebook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       // public MainWindowViewModel MainWindowViewModel { get; private set; }
        public static App Instance { get; private set; } = null!;

        public App()
        {
            Instance = this;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
           // MainWindowViewModel = new();
            base.OnStartup(e);
        }
    }
}