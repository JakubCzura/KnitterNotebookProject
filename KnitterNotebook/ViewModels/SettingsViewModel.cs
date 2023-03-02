using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Themes;
using KnitterNotebook.Validators;
using KnitterNotebook.Views.UserControls;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
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

       

        // private string newNickname = string.Empty;
        private string newNickname;

        public string NewNickname
        {
            get { return newNickname; }
            set { newNickname = value; OnPropertyChanged(); }
        }

        private string newEmail;

        public string NewEmail
        {
            get { return newEmail; }
            set { newEmail = value; OnPropertyChanged(); }
        }

        private string newTheme;

        public string NewTheme
        {
            get { return newTheme; }
            set { newTheme = value; OnPropertyChanged(); }
        }

        private Visibility userSettingsUserControlVisibility = Visibility.Visible;

        public Visibility UserSettingsUserControlVisibility
        {
            get { return userSettingsUserControlVisibility; }
            set { userSettingsUserControlVisibility = value; OnPropertyChanged(); }
        }

        private Visibility themeSettingsUserControlVisibility = Visibility.Hidden;

        public Visibility ThemeSettingsUserControlVisibility
        {
            get { return themeSettingsUserControlVisibility; }
            set { themeSettingsUserControlVisibility = value; OnPropertyChanged(); }
        }

        private IEnumerable themes;

        public IEnumerable Themes
        {
            get { return themes; }
            set { themes = value; OnPropertyChanged(); }
        }

        public ICommand SetUserSettingsUserControlVisibleCommand { get; private set; }

        public ICommand SetThemeSettingsUserControlVisibleCommand { get; private set; }

        public ICommand ChangeNicknameCommandAsync { get; private set; }

        public ICommand ChangeEmailCommandAsync { get; private set; }

        public ICommand ChangePasswordCommandAsync { get; private set; }

        public ICommand ChangeThemeCommandAsync { get; private set; }

        private KnitterNotebookContext KnitterNotebookContext { get; set; }   

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

        private async Task ChangeNicknameAsync()
        {
            try
            {
                using (KnitterNotebookContext = new())
                {
                    User? user = await KnitterNotebookContext.Users.FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.LoggedUserId);
                    if (user == null)
                    {
                        MessageBox.Show("Błąd zmiany nicku");
                    }
                    else
                    {
                        user.Nickname = NewNickname;
                        KnitterNotebookContext.Users.Update(user);
                        await KnitterNotebookContext.SaveChangesAsync();
                        MessageBox.Show($"Zmieniono nick na: {user.Nickname}");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private async Task ChangeEmailAsync()
        {
            try
            {
                using (KnitterNotebookContext = new())
                {
                    User? user = await KnitterNotebookContext.Users.FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.LoggedUserId);
                    if (user == null)
                    {
                        MessageBox.Show("Błąd zmiany e-mail");
                    }
                    else
                    {
                        user.Email = NewEmail;
                        KnitterNotebookContext.Users.Update(user);
                        await KnitterNotebookContext.SaveChangesAsync();
                        MessageBox.Show($"Zmieniono e-mail na {user.Email}");
                    }
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
                using (KnitterNotebookContext = new())
                {
                    User? user = await KnitterNotebookContext.Users.Include(x => x.Theme).FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.LoggedUserId);
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
                                KnitterNotebookContext.Users.Update(user);
                                await KnitterNotebookContext.SaveChangesAsync();
                                MessageBox.Show($"Zmieniono hasło");
                            }
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
                using (KnitterNotebookContext = new())
                {
                    User? user = await KnitterNotebookContext.Users.FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.LoggedUserId);
                    Theme? theme = await KnitterNotebookContext.Themes.FirstOrDefaultAsync(x => x.Name == NewTheme);
                    if (user == null || theme == null)
                    {
                        MessageBox.Show("Błąd zmiany motywu");
                    }
                    else
                    {
                        user.Theme = theme;
                        KnitterNotebookContext.Users.Update(user);
                        await KnitterNotebookContext.SaveChangesAsync();
                        ThemeChanger.SetTheme();
                        MessageBox.Show($"Zmieniono interfejs aplikacji na {user.Theme.Name}");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}