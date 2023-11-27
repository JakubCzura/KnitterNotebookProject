using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels;

/// <summary>
/// View model for RegistrationWindow.xaml
/// </summary>
public partial class RegistrationViewModel(ILogger<RegistrationViewModel> logger,
    IUserService userService,
    IValidator<RegisterUserDto> registerUserDtoValidator) : BaseViewModel
{
    private readonly ILogger<RegistrationViewModel> _logger = logger;
    private readonly IUserService _userService = userService;
    private readonly IValidator<RegisterUserDto> _registerUserDtoValidator = registerUserDtoValidator;

    #region Properties

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _nickname = string.Empty;

    #endregion Properties

    #region Commands

    [RelayCommand]
    private async Task RegisterUser()
    {
        try
        {
            RegisterUserDto registerUserDto = new(Nickname, Email, RegistrationWindow.Instance.UserPasswordPasswordBox.Password);
            ValidationResult validation = await _registerUserDtoValidator.ValidateAsync(registerUserDto);
            if (!validation.IsValid)
            {
                string errorMessage = validation.Errors.GetMessagesAsString();
                MessageBox.Show(errorMessage);
                return;
            }
            await _userService.CreateAsync(registerUserDto);
            Closewindow(RegistrationWindow.Instance);
            MessageBox.Show(Translations.YouHaveBeenSignedUp);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while signing up");
            MessageBox.Show(Translations.ErrorWhileSigningUp);
        }
        finally
        {
            if (RegistrationWindow.Instance is not null)
            {
                RegistrationWindow.Instance.UserPasswordPasswordBox.Password = string.Empty;
            }
        }
    }

    #endregion Commands
}