using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KnitterNotebook.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ThemeSettingsUserControl.xaml
    /// </summary>
    public partial class ThemeSettingsUserControl : UserControl
    {
        public ThemeSettingsUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty NewThemeProperty =
            DependencyProperty.Register(nameof(NewTheme), typeof(string), typeof(ThemeSettingsUserControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string NewTheme
        {
            get { return GetValue(NewThemeProperty).ToString()!; }
            set { SetValue(NewThemeProperty, value); }
        }

        public static readonly DependencyProperty ThemesProperty =
            DependencyProperty.Register(nameof(Themes), typeof(IEnumerable), typeof(ThemeSettingsUserControl),
                new FrameworkPropertyMetadata(default));

        public IEnumerable Themes
        {
            get { return (GetValue(ThemesProperty) as IEnumerable)!; }
            set { SetValue (ThemesProperty, value); }
        }


        public static readonly DependencyProperty ChangeThemeCommandAsyncProperty =
          DependencyProperty.Register(nameof(ChangeThemeCommandAsync), typeof(ICommand), typeof(ThemeSettingsUserControl),
              new FrameworkPropertyMetadata(null));

        public ICommand ChangeThemeCommandAsync
        {
            get { return (GetValue(ChangeThemeCommandAsyncProperty) as ICommand)!; }
            set { SetValue(ChangeThemeCommandAsyncProperty, value); }
        }
    }
}