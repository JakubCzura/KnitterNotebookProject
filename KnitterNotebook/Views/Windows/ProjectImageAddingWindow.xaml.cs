using System.Windows;

namespace KnitterNotebook.Views.Windows;

/// <summary>
/// Interaction logic for ProjectImageAddingWindow.xaml
/// </summary>
public partial class ProjectImageAddingWindow : Window
{
    public static ProjectImageAddingWindow Instance { get; private set; } = null!;

    public ProjectImageAddingWindow()
    {
        InitializeComponent();
        Instance = this;
    }
}