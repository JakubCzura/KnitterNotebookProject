using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels
{
    public partial class ResetPasswordViewModel : BaseViewModel
    {
        public ResetPasswordViewModel(ILogger<ResetPasswordViewModel> logger, IConfiguration configuration, IUserService userService, IValidator<ResetPasswordDto> resetPasswordDtoValidator, IEmailService emailService)
        {
            _logger = logger;
            _configuration = configuration;
            _userService = userService;
            _resetPasswordDtoValidator = resetPasswordDtoValidator;
            _emailService = emailService;
        }

        private readonly ILogger<ResetPasswordViewModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IValidator<ResetPasswordDto> _resetPasswordDtoValidator;
        private readonly IEmailService _emailService;
        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _token = string.Empty;

        [RelayCommand]
        private async Task SendPasswordResetTokenEmailAsync()
        {
            throw new NotImplementedException("SendPasswordResetTokenEmailAsync - reset password view model");
        }

        [RelayCommand]
        private async Task ResetPasswordAsync()
        {
            try
            {
                ResetPasswordDto resetPasswordDto = new(Email,
                                                        ResetPasswordWindow.Instance.NewPasswordPasswordBox.Password,
                                                        ResetPasswordWindow.Instance.RepeatedNewPasswordPasswordBox.Password);
                ValidationResult validation = await _resetPasswordDtoValidator.ValidateAsync(resetPasswordDto);
                if (!validation.IsValid)
                {
                    string errorMessage = validation.Errors.GetMessagesAsString();
                    MessageBox.Show(errorMessage, "Błąd zmiany hasła");
                    return;
                }
                await _userService.ResetPasswordAsync(resetPasswordDto);
                MessageBox.Show($"Ustawiono nowe hasło");
                Closewindow<ResetPasswordWindow>();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while resetting password");
                MessageBox.Show(exception.Message);
            }
            finally
            {
                Email = string.Empty;
                ResetPasswordWindow.Instance.NewPasswordPasswordBox.Password = string.Empty;
                ResetPasswordWindow.Instance.RepeatedNewPasswordPasswordBox.Password = string.Empty;
            }
        }
    }
}