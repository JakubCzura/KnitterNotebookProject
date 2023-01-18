using System.Windows;

namespace KnitterNotebook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // public MainViewModel MainViewModel { get; private set; }
        public static App Instance { get; private set; } = null!;

        public App()
        {
            Instance = this;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // MainViewModel = new();
            base.OnStartup(e);
        }
    }
}