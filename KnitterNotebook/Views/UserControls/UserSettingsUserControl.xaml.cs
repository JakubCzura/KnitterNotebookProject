using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KnitterNotebook.Views.UserControls
{
    /// <summary>
    /// Interaction logic for UserSettingsUserControl.xaml
    /// </summary>
    public partial class UserSettingsUserControl : UserControl
    {
        public static UserSettingsUserControl Instance { get; private set; } = null!;

        public UserSettingsUserControl()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}