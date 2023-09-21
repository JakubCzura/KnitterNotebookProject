using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels;

/// <summary>
/// View model for LoginWindow.xaml
/// </summary>
public partial class LoginViewModel : BaseViewModel
{
    public LoginViewModel(ILogger<LoginViewModel> logger, IUserService userService, IValidator<LogInDto> logInDtoValidator, SharedResourceViewModel sharedResourceViewModel)
    {
        _logger = logger;
        _userService = userService;
        _logInDtoValidator = logInDtoValidator;
        _sharedResourceViewModel = sharedResourceViewModel;
    }

    private readonly ILogger<LoginViewModel> _logger;
    private readonly IUserService _userService;
    private readonly IValidator<LogInDto> _logInDtoValidator;
    private readonly SharedResourceViewModel _sharedResourceViewModel;

    #region Properties

    [ObservableProperty]
    private string _email = string.Empty;

    public ICommand ShowRegistrationWindowCommand { get; } = new RelayCommand(ShowWindow<RegistrationWindow>);

    public ICommand ShowResetPasswordWindowCommand { get; } = new RelayCommand(ShowWindow<ResetPasswordWindow>);

    #endregion Properties

    #region Methods

    [RelayCommand]
    private async Task LogInAsync()
    {
        try
        {
            LogInDto logInDto = new(Email, LoginWindow.Instance.UserPasswordPasswordBox.Password);

            ValidationResult validation = _logInDtoValidator.Validate(logInDto);
            if (!validation.IsValid)
            {
                MessageBox.Show(Translations.InvalidLoginCredentials);
                return;
            }

            int? userId = await _userService.LogInAsync(logInDto);
            if (!userId.HasValue)
            {
                MessageBox.Show(Translations.UserNotFound);
                return;
            }

            _sharedResourceViewModel.UserId = userId.Value;

            ShowWindow<MainWindow>();
            Closewindow(LoginWindow.Instance);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while signing in");
            MessageBox.Show(Translations.ErrorWhileSigninIn);
        }
        finally
        {
            Email = string.Empty;
            if (LoginWindow.Instance != null)
            {
                LoginWindow.Instance.UserPasswordPasswordBox.Password = string.Empty;
            }
        }
    }

    #endregion Methods
}