using CommunityToolkit.Mvvm.Input;
using FluentValidation;
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
using System.Windows.Controls;
using System.Windows.Input;
using ValidationResult = FluentValidation.Results.ValidationResult;

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
            ChooseUserSettingsUserControlCommand = new RelayCommand(() => SettingsWindowContent = new UserSettingsUserControl());
            ChooseThemeSettingsUserControlCommand = new RelayCommand(() => SettingsWindowContent = new ThemeSettingsUserControl());
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

        public ICommand ChooseThemeSettingsUserControlCommand { get; }

        public ICommand ChooseUserSettingsUserControlCommand { get; }

        private UserControl _settingsWindowContent = new UserSettingsUserControl();

        public UserControl SettingsWindowContent
        {
            get => _settingsWindowContent;
            set { _settingsWindowContent = value; OnPropertyChanged(); }
        }

        public string NewEmail
        {
            get => _newEmail;
            set { _newEmail = value; OnPropertyChanged(); }
        }

        public string NewNickname
        {
            get => _newNickname;
            set { _newNickname = value; OnPropertyChanged(); }
        }

        public string NewTheme
        {
            get => _newTheme;
            set { _newTheme = value; OnPropertyChanged(); }
        }

        public IEnumerable<string> Themes
        {
            get => _themes;
            set { _themes = value; OnPropertyChanged(); }
        }

        private async Task ChangeEmailAsync()
        {
            try
            {
                ChangeEmailDto changeEmailDto = new(LoggedUserInformation.Id, NewEmail);
                ValidationResult validation = await _changeEmailDtoValidator.ValidateAsync(changeEmailDto);
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd zmiany email");
                    return;
                }
                await _userService.ChangeEmailAsync(changeEmailDto);
                MessageBox.Show($"Zmieniono email na: {changeEmailDto.Email}");
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
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd zmiany nazwy użytkownika");
                    return;
                }
                await _userService.ChangeNicknameAsync(changeNicknameDto);
                MessageBox.Show($"Zmieniono nazwę użytkownika");
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
                ValidationResult validation = await _changePasswordDtoValidator.ValidateAsync(changePasswordDto);
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd zmiany hasła");
                    return;
                }
                await _userService.ChangePasswordAsync(changePasswordDto);
                MessageBox.Show($"Zmieniono hasło");
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
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd zmiany motywu");
                    return;
                }
                await _userService.ChangeThemeAsync(changeThemeDto);
                string themeFullName = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, $"Themes/{NewTheme}Mode.xaml");
                ThemeChanger.SetTheme(themeFullName);
                MessageBox.Show($"Zmieniono interfejs aplikacji na {NewTheme}");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}