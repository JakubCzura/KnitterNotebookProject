using System.Windows;

namespace KnitterNotebook.Views.Windows;

/// <summary>
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{
    public static LoginWindow Instance { get; private set; } = null!;

    public LoginWindow()
    {
        InitializeComponent();
        Instance = this;
    }
}