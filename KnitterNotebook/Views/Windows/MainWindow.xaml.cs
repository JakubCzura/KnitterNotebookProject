using System.Windows;

namespace KnitterNotebook.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; } = null!;
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}