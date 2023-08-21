using System.Windows;

namespace KnitterNotebook.Views.Windows
{
    /// <summary>
    /// Interaction logic for SampleAddingWindow.xaml
    /// </summary>
    public partial class SampleAddingWindow : Window
    {
        public static SampleAddingWindow Instance { get; private set; } = null!;

        public SampleAddingWindow()
        {
            InitializeComponent();
            Instance = this;
        }
    }
}