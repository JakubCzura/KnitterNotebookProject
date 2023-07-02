using System.Windows;

namespace KnitterNotebook.Views.Windows
{
    /// <summary>
    /// Interaction logic for ResetPasswordWindow.xaml
    /// </summary>
    public partial class ResetPasswordWindow : Window
    {
        public static ResetPasswordWindow Instance { get; private set; } = null!;

        public ResetPasswordWindow()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}