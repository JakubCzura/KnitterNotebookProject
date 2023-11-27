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
public partial class LoginViewModel(ILogger<LoginViewModel> logger,
    IUserService userService,
    IValidator<LogInDto> logInDtoValidator,
    SharedResourceViewModel sharedResourceViewModel) : BaseViewModel
{
    private readonly ILogger<LoginViewModel> _logger = logger;
    private readonly IUserService _userService = userService;
    private readonly IValidator<LogInDto> _logInDtoValidator = logInDtoValidator;
    private readonly SharedResourceViewModel _sharedResourceViewModel = sharedResourceViewModel;

    #region Properties

    [ObservableProperty]
    private string _email = string.Empty;

    #endregion Properties

    #region Commands

    public ICommand ShowRegistrationWindowCommand { get; } = new RelayCommand(ShowWindow<RegistrationWindow>);

    public ICommand ShowResetPasswordWindowCommand { get; } = new RelayCommand(ShowWindow<ResetPasswordWindow>);

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

    #endregion Commands
}