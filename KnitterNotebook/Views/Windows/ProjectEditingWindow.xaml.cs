using System.Windows;

namespace KnitterNotebook.Views.Windows
{
    /// <summary>
    /// Interaction logic for ProjectEditingWindow.xaml
    /// </summary>
    public partial class ProjectEditingWindow : Window
    {
        public static ProjectEditingWindow Instance { get; private set; } = null!;

        public ProjectEditingWindow()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}