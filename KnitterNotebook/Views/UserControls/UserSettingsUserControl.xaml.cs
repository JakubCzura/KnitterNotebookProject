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
        public UserSettingsUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty NewNicknameProperty =
            DependencyProperty.Register(nameof(NewNickname), typeof(string), typeof(UserSettingsUserControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string NewNickname
        {
            get { return GetValue(NewNicknameProperty).ToString()!; }
            set { SetValue(NewNicknameProperty, value); }
        }

        public static readonly DependencyProperty ChangeNicknameCommandAsyncProperty =
            DependencyProperty.Register(nameof(ChangeNicknameCommandAsync), typeof(ICommand), typeof(UserSettingsUserControl),
                new FrameworkPropertyMetadata(null));

        public ICommand ChangeNicknameCommandAsync
        {
            get { return (GetValue(ChangeNicknameCommandAsyncProperty) as ICommand)!; }
            set { SetValue(ChangeNicknameCommandAsyncProperty, value); }
        }

        public static readonly DependencyProperty NewEmailProperty =
            DependencyProperty.Register(nameof(NewEmail), typeof(string), typeof(UserSettingsUserControl),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string NewEmail
        {
            get { return GetValue(NewEmailProperty).ToString()!; }
            set { SetValue(NewEmailProperty, value); }
        }

        public static readonly DependencyProperty ChangeEmailCommandAsyncProperty =
           DependencyProperty.Register(nameof(ChangeEmailCommandAsync), typeof(ICommand), typeof(UserSettingsUserControl),
               new FrameworkPropertyMetadata(null));

        public ICommand ChangeEmailCommandAsync
        {
            get { return (GetValue(ChangeEmailCommandAsyncProperty) as ICommand)!; }
            set { SetValue(ChangeEmailCommandAsyncProperty, value); }
        }
    }
}