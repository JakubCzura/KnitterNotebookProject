using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
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
    public partial class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(IUserService userService,
            IValidator<ChangeNicknameDto> changeNicknameDtoValidator,
            IValidator<ChangeEmailDto> changeEmailDtoValidator,
            IValidator<ChangePasswordDto> changePasswordDtoValidator,
            IValidator<ChangeThemeDto> changeThemeDtoValidator)
        {
            _userService = userService;
            _changeNicknameDtoValidator = changeNicknameDtoValidator;
            _changeEmailDtoValidator = changeEmailDtoValidator;
            _changePasswordDtoValidator = changePasswordDtoValidator;
            _changeThemeDtoValidator = changeThemeDtoValidator;
            ChooseUserSettingsUserControlCommand = new RelayCommand(() => SettingsWindowContent = new UserSettingsUserControl());
            ChooseThemeSettingsUserControlCommand = new RelayCommand(() => SettingsWindowContent = new ThemeSettingsUserControl());
        }

        private readonly IUserService _userService;

        private readonly IValidator<ChangeNicknameDto> _changeNicknameDtoValidator;

        private readonly IValidator<ChangeEmailDto> _changeEmailDtoValidator;

        private readonly IValidator<ChangePasswordDto> _changePasswordDtoValidator;

        private readonly IValidator<ChangeThemeDto> _changeThemeDtoValidator;

        public ICommand ChooseThemeSettingsUserControlCommand { get; }

        public ICommand ChooseUserSettingsUserControlCommand { get; }

        [ObservableProperty]
        private UserControl _settingsWindowContent = new UserSettingsUserControl();

        [ObservableProperty]
        private string _newEmail = string.Empty;

        [ObservableProperty]
        private string _newNickname = string.Empty;

        [ObservableProperty]
        private string _newTheme = string.Empty;

        [ObservableProperty]
        private IEnumerable<string> _themes = ApplicationThemes.ThemesList();

        [RelayCommand]
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
            finally
            {
                NewEmail = string.Empty;
            }
        }

        [RelayCommand]
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
            finally
            {
                NewNickname = string.Empty;
            }
        }

        [RelayCommand]
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

        [RelayCommand]
        private async Task ChangeThemeAsync()
        {
            try
            {
                ChangeThemeDto changeThemeDto = new(LoggedUserInformation.Id, NewTheme);
                ValidationResult validation = await _changeThemeDtoValidator.ValidateAsync(changeThemeDto);
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