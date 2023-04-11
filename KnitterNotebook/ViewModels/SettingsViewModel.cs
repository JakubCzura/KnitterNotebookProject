using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Themes;
using KnitterNotebook.Validators;
using KnitterNotebook.Views.UserControls;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly KnitterNotebookContext _knitterNotebookContext;

        private string _newEmail = string.Empty;

        // private string newNickname = string.Empty;
        private string _newNickname = string.Empty;

        private string _newTheme = string.Empty;

        private IEnumerable _themes = Enumerable.Empty<string>();

        private Visibility _themeSettingsUserControlVisibility = Visibility.Hidden;

        private Visibility _userSettingsUserControlVisibility = Visibility.Visible;

        public SettingsViewModel(KnitterNotebookContext knitterNotebookContext)
        {
            _knitterNotebookContext = knitterNotebookContext;
            SetUserSettingsUserControlVisibleCommand = new RelayCommand(() =>
            {
                SetUserControlsVisibilityHidden(); UserSettingsUserControlVisibility = Visibility.Visible;
            });
            SetThemeSettingsUserControlVisibleCommand = new RelayCommand(() =>
            {
                SetUserControlsVisibilityHidden(); ThemeSettingsUserControlVisibility = Visibility.Visible;
            });
            Themes = ApplicationThemes.GetThemes();
            ChangeNicknameCommandAsync = new AsyncRelayCommand(ChangeNicknameAsync);
            ChangeEmailCommandAsync = new AsyncRelayCommand(ChangeEmailAsync);
            ChangePasswordCommandAsync = new AsyncRelayCommand(ChangePasswordAsync);
            ChangeThemeCommandAsync = new AsyncRelayCommand(ChangeThemeAsync);
        }

        public ICommand ChangeEmailCommandAsync { get; }

        public ICommand ChangeNicknameCommandAsync { get; }

        public ICommand ChangePasswordCommandAsync { get; }

        public ICommand ChangeThemeCommandAsync { get; }

        public ICommand SetThemeSettingsUserControlVisibleCommand { get; }

        public ICommand SetUserSettingsUserControlVisibleCommand { get; }

        public string NewEmail
        {
            get { return _newEmail; }
            set { _newEmail = value; OnPropertyChanged(); }
        }

        public string NewNickname
        {
            get { return _newNickname; }
            set { _newNickname = value; OnPropertyChanged(); }
        }

        public string NewTheme
        {
            get { return _newTheme; }
            set { _newTheme = value; OnPropertyChanged(); }
        }     

        public IEnumerable Themes
        {
            get { return _themes; }
            set { _themes = value; OnPropertyChanged(); }
        }

        public Visibility ThemeSettingsUserControlVisibility
        {
            get { return _themeSettingsUserControlVisibility; }
            set { _themeSettingsUserControlVisibility = value; OnPropertyChanged(); }
        }

        public Visibility UserSettingsUserControlVisibility
        {
            get { return _userSettingsUserControlVisibility; }
            set { _userSettingsUserControlVisibility = value; OnPropertyChanged(); }
        }

        private async Task ChangeEmailAsync()
        {
            try
            {
                User? user = await _knitterNotebookContext.Users.FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.LoggedUserId);
                if (user == null)
                {
                    MessageBox.Show("Błąd zmiany e-mail");
                }
                else
                {
                    user.Email = NewEmail;
                    _knitterNotebookContext.Users.Update(user);
                    await _knitterNotebookContext.SaveChangesAsync();
                    MessageBox.Show($"Zmieniono e-mail na {user.Email}");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private async Task ChangeNicknameAsync()
        {
            try
            {
                User? user = await _knitterNotebookContext.Users.FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.LoggedUserId);
                if (user == null)
                {
                    MessageBox.Show("Błąd zmiany nicku");
                }
                else
                {
                    user.Nickname = NewNickname;
                    _knitterNotebookContext.Users.Update(user);
                    await _knitterNotebookContext.SaveChangesAsync();
                    MessageBox.Show($"Zmieniono nick na: {user.Nickname}");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private async Task ChangePasswordAsync()
        {
            try
            {
                User? user = await _knitterNotebookContext.Users.Include(x => x.Theme).FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.LoggedUserId);
                if (user == null)
                {
                    MessageBox.Show("Błąd zmiany hasła");
                }
                else
                {
                    if (UserSettingsUserControl.Instance.NewPasswordPasswordBox.Password !=
                        UserSettingsUserControl.Instance.RepeatedNewPasswordPasswordBox.Password)
                    {
                        MessageBox.Show("Hasła nie są identyczne");
                    }
                    else
                    {
                        UserValidator userValidator = new();
                        user.Password = UserSettingsUserControl.Instance.NewPasswordPasswordBox.Password;
                        if (userValidator.Validate(user))
                        {
                            user.Password = PasswordHasher.HashPassword(user.Password);
                            _knitterNotebookContext.Users.Update(user);
                            await _knitterNotebookContext.SaveChangesAsync();
                            MessageBox.Show($"Zmieniono hasło");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private async Task ChangeThemeAsync()
        {
            try
            {
                User? user = await _knitterNotebookContext.Users.FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.LoggedUserId);
                Theme? theme = await _knitterNotebookContext.Themes.FirstOrDefaultAsync(x => x.Name == NewTheme);
                if (user == null || theme == null)
                {
                    MessageBox.Show("Błąd zmiany motywu");
                }
                else
                {
                    user.Theme = theme;
                    _knitterNotebookContext.Users.Update(user);
                    await _knitterNotebookContext.SaveChangesAsync();
                    string themeFullName = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, $"Themes/{user.Theme.Name}Mode.xaml");
                    ThemeChanger.SetTheme(themeFullName);
                    MessageBox.Show($"Zmieniono interfejs aplikacji na {user.Theme.Name}");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SetUserControlsVisibilityHidden()
        {
            try
            {
                UserSettingsUserControlVisibility = Visibility.Hidden;
                ThemeSettingsUserControlVisibility = Visibility.Hidden;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd wyboru zawartości okna ustawień");
            }
        }
    }
}