using System.Windows;

namespace KnitterNotebook.Views.Windows
{
    /// <summary>
    /// Interaction logic for ProjectInProgressEditingWindow.xaml
    /// </summary>
    public partial class ProjectInProgressEditingWindow : Window
    {
        public static ProjectInProgressEditingWindow Instance { get; private set; } = null!;

        public ProjectInProgressEditingWindow()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}