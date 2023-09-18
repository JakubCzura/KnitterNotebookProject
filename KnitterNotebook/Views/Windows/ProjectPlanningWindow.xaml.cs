using System.Windows;

namespace KnitterNotebook.Views.Windows;

/// <summary>
/// Interaction logic for ProjectPlanningWindow.xaml
/// </summary>
public partial class ProjectPlanningWindow : Window
{
    public static ProjectPlanningWindow Instance { get; private set; } = null!;

    public ProjectPlanningWindow()
    {
        InitializeComponent();
        Instance = this;
    }
}