using System.Windows;

namespace KnitterNotebook.Views.Windows
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public static RegistrationWindow Instance { get; private set; } = null!;

        public RegistrationWindow()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}