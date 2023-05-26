using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Themes;
using KnitterNotebook.Views.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly DatabaseContext _databaseContext;

        private readonly IUserService _userService;

        private readonly IValidator<ChangeNicknameDto> _changeNicknameDtoValidator;

        private readonly IValidator<ChangeEmailDto> _changeEmailDtoValidator;

        private readonly IValidator<ChangePasswordDto> _changePasswordDtoValidator;

        private readonly IValidator<ChangeThemeDto> _changeThemeDtoValidator;

        private readonly IThemeService _themeService;

        private string _newEmail = string.Empty;

        private string _newNickname = string.Empty;

        private string _newTheme = string.Empty;

        private IEnumerable<string> _themes = Enumerable.Empty<string>();

        private Visibility _themeSettingsUserControlVisibility = Visibility.Hidden;

        private Visibility _userSettingsUserControlVisibility = Visibility.Visible;

        public SettingsViewModel(DatabaseContext databaseContext,
            IUserService userService,
            IValidator<ChangeNicknameDto> changeNicknameDtoValidator,
            IValidator<ChangeEmailDto> changeEmailDtoValidator,
            IValidator<ChangePasswordDto> changePasswordDtoValidator,
            IValidator<ChangeThemeDto> changeThemeDtoValidator,
            IThemeService themeService)
        {
            _databaseContext = databaseContext;
            _userService = userService;
            _changeNicknameDtoValidator = changeNicknameDtoValidator;
            _changeEmailDtoValidator = changeEmailDtoValidator;
            _changePasswordDtoValidator = changePasswordDtoValidator;
            _changeThemeDtoValidator = changeThemeDtoValidator;
            _themeService = themeService;
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

        public IEnumerable<string> Themes
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
                ChangeEmailDto changeEmailDto = new(LoggedUserInformation.Id, NewEmail);
                ValidationResult validation = await _changeEmailDtoValidator.ValidateAsync(changeEmailDto);
                if (validation.IsValid)
                {
                    await _userService.ChangeEmailAsync(changeEmailDto);
                    MessageBox.Show($"Zmieniono email na: {changeEmailDto.Email}");
                }
                else
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd zmiany email");
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
                ChangeNicknameDto changeNicknameDto = new(LoggedUserInformation.Id, NewNickname);
                ValidationResult validation = await _changeNicknameDtoValidator.ValidateAsync(changeNicknameDto);
                if (validation.IsValid)
                {
                    await _userService.ChangeNicknameAsync(changeNicknameDto);
                    MessageBox.Show($"Zmieniono nazwę użytkownika");
                }
                else
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd zmiany nazwy użytkownika");
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
                ChangePasswordDto changePasswordDto = new(LoggedUserInformation.Id,
                    UserSettingsUserControl.Instance.NewPasswordPasswordBox.Password,
                    UserSettingsUserControl.Instance.RepeatedNewPasswordPasswordBox.Password);
                ValidationResult validation = _changePasswordDtoValidator.Validate(changePasswordDto);
                if (validation.IsValid)
                {
                    await _userService.ChangePasswordAsync(changePasswordDto);
                    MessageBox.Show($"Zmieniono hasło");
                }
                else
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd zmiany hasła");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                UserSettingsUserControl.Instance.NewPasswordPasswordBox.Password = string.Empty;
                UserSettingsUserControl.Instance.RepeatedNewPasswordPasswordBox.Password = string.Empty;
            }
        }

        private async Task ChangeThemeAsync()
        {
            try
            {
                ChangeThemeDto changeThemeDto = new(LoggedUserInformation.Id, NewTheme);
                ValidationResult validation = _changeThemeDtoValidator.Validate(changeThemeDto);
                if (validation.IsValid)
                {
                    await _userService.ChangeThemeAsync(changeThemeDto);
                    string themeFullName = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, $"Themes/{NewTheme}Mode.xaml");
                    ThemeChanger.SetTheme(themeFullName);
                    MessageBox.Show($"Zmieniono interfejs aplikacji na {NewTheme}");
                }
                else
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd zmiany motywu");
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