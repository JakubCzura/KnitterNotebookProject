using System.Windows;

namespace KnitterNotebook.Views.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public static SettingsWindow Instance { get; private set; } = null!;
        public SettingsWindow()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}