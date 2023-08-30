using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        public RegistrationViewModel(ILogger<RegistrationViewModel> logger, IUserService userService, IValidator<RegisterUserDto> registerUserDtoValidator)
        {
            _logger = logger;
            _userService = userService;
            _registerUserDtoValidator = registerUserDtoValidator;
        }

        #region Properties

        private readonly ILogger<RegistrationViewModel> _logger;
        private readonly IUserService _userService;
        private readonly IValidator<RegisterUserDto> _registerUserDtoValidator;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _nickname = string.Empty;

        #endregion Properties

        #region Methods

        [RelayCommand]
        private async Task RegisterUser()
        {
            try
            {
                RegisterUserDto registerUserDto = new(Nickname, Email, RegistrationWindow.Instance.UserPasswordPasswordBox.Password);
                ValidationResult validation = await _registerUserDtoValidator.ValidateAsync(registerUserDto);
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd podczas rejestracji");
                    return;
                }
                await _userService.CreateAsync(registerUserDto);
                Closewindow(RegistrationWindow.Instance);
                MessageBox.Show("Rejestracja przebiegła pomyślnie");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while registering user");
                MessageBox.Show(exception.Message);
            }
        }

        #endregion Methods
    }
}