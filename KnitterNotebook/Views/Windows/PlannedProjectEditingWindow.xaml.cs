using System.Windows;

namespace KnitterNotebook.Views.Windows
{
    /// <summary>
    /// Interaction logic for PlannedProjectEditingWindow.xaml
    /// </summary>
    public partial class PlannedProjectEditingWindow : Window
    {
        public static PlannedProjectEditingWindow Instance { get; private set; } = null!;

        public PlannedProjectEditingWindow()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}