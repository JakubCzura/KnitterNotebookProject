using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.UserControls;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;
using UserControl = System.Windows.Controls.UserControl;

namespace KnitterNotebook.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    public SettingsViewModel(ILogger<SettingsViewModel> logger,
        IUserService userService,
        IValidator<ChangeNicknameDto> changeNicknameDtoValidator,
        IValidator<ChangeEmailDto> changeEmailDtoValidator,
        IValidator<ChangePasswordDto> changePasswordDtoValidator,
        IValidator<ChangeThemeDto> changeThemeDtoValidator,
        IWindowContentService windowContentService,
        SharedResourceViewModel sharedResourceViewModel)
    {
        _logger = logger;
        _userService = userService;
        _changeNicknameDtoValidator = changeNicknameDtoValidator;
        _changeEmailDtoValidator = changeEmailDtoValidator;
        _changePasswordDtoValidator = changePasswordDtoValidator;
        _changeThemeDtoValidator = changeThemeDtoValidator;
        _windowContentService = windowContentService;
        _sharedResourceViewModel = sharedResourceViewModel;
    }

    private readonly ILogger<SettingsViewModel> _logger;
    private readonly IUserService _userService;
    private readonly IValidator<ChangeNicknameDto> _changeNicknameDtoValidator;
    private readonly IValidator<ChangeEmailDto> _changeEmailDtoValidator;
    private readonly IValidator<ChangePasswordDto> _changePasswordDtoValidator;
    private readonly IValidator<ChangeThemeDto> _changeThemeDtoValidator;
    private readonly IWindowContentService _windowContentService;
    private readonly SharedResourceViewModel _sharedResourceViewModel;

    [ObservableProperty]
    private UserControl _settingsWindowContent = new UserSettingsUserControl();

    [ObservableProperty]
    private string _newEmail = string.Empty;

    [ObservableProperty]
    private string _newNickname = string.Empty;

    [ObservableProperty]
    private ApplicationTheme _newTheme = ApplicationTheme.Default;

    [ObservableProperty]
    private string[] _themes = Enum.GetNames(typeof(ApplicationTheme));

    [RelayCommand]
    private async Task ChangeEmailAsync()
    {
        try
        {
            ChangeEmailDto changeEmailDto = new(_sharedResourceViewModel.UserId, NewEmail);
            ValidationResult validation = await _changeEmailDtoValidator.ValidateAsync(changeEmailDto);
            if (!validation.IsValid)
            {
                string errorMessage = validation.Errors.GetMessagesAsString();
                MessageBox.Show(errorMessage, "Błąd zmiany email");
                return;
            }
            await _userService.ChangeEmailAsync(changeEmailDto);
            _sharedResourceViewModel.OnUserUpdatedInDatabase();
            MessageBox.Show($"Zmieniono email na: {changeEmailDto.Email}");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while changing e-mail");
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
            ChangeNicknameDto changeNicknameDto = new(_sharedResourceViewModel.UserId, NewNickname);
            ValidationResult validation = await _changeNicknameDtoValidator.ValidateAsync(changeNicknameDto);
            if (!validation.IsValid)
            {
                string errorMessage = validation.Errors.GetMessagesAsString();
                MessageBox.Show(errorMessage, "Błąd zmiany nazwy użytkownika");
                return;
            }
            await _userService.ChangeNicknameAsync(changeNicknameDto);
            _sharedResourceViewModel.OnUserUpdatedInDatabase();
            MessageBox.Show($"Zmieniono nazwę użytkownika");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while changing nickname");
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
            ChangePasswordDto changePasswordDto = new(_sharedResourceViewModel.UserId,
                                                    UserSettingsUserControl.Instance.NewPasswordPasswordBox.Password,
                                                    UserSettingsUserControl.Instance.RepeatedNewPasswordPasswordBox.Password);
            ValidationResult validation = await _changePasswordDtoValidator.ValidateAsync(changePasswordDto);
            if (!validation.IsValid)
            {
                string errorMessage = validation.Errors.GetMessagesAsString();
                MessageBox.Show(errorMessage, "Błąd zmiany hasła");
                return;
            }
            await _userService.ChangePasswordAsync(changePasswordDto);
            MessageBox.Show($"Zmieniono hasło");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while changing password");
            MessageBox.Show(exception.Message);
        }
        finally
        {
            if (UserSettingsUserControl.Instance is not null)
            {
                UserSettingsUserControl.Instance.NewPasswordPasswordBox.Password = string.Empty;
                UserSettingsUserControl.Instance.RepeatedNewPasswordPasswordBox.Password = string.Empty;
            }
        }
    }

    [RelayCommand]
    private async Task ChangeThemeAsync()
    {
        try
        {
            ChangeThemeDto changeThemeDto = new(_sharedResourceViewModel.UserId, NewTheme);
            ValidationResult validation = await _changeThemeDtoValidator.ValidateAsync(changeThemeDto);
            if (!validation.IsValid)
            {
                string errorMessage = validation.Errors.GetMessagesAsString();
                MessageBox.Show(errorMessage, "Błąd zmiany motywu");
                return;
            }
            await _userService.ChangeThemeAsync(changeThemeDto);
            MessageBox.Show($"Zmieniono interfejs aplikacji na {NewTheme}");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while changing theme");
            _sharedResourceViewModel.OnUserUpdatedInDatabase();
            MessageBox.Show(exception.Message);
        }
    }

    [RelayCommand]
    private void ChooseSettingsWindowContent(SettingsWindowContent userControlName) => SettingsWindowContent = _windowContentService.ChooseSettingsWindowContent(userControlName);
}