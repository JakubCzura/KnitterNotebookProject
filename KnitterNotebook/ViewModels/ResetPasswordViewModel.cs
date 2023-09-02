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
            bool emailExists = await _userService.IsEmailTakenAsync(Email);
            if (!emailExists)
            {
                MessageBox.Show("Nie odnaleziono adresu e-mail", "Błąd wysłania tokenu");
                return;
            }

            (string, DateTime) tokenWithExpirationDate = await _userService.UpdatePasswordResetTokenStatusAsync(Email);

            SendEmailDto sendEmailDto = new()
            {
                To = Email,
                Subject = "Password reset",
                Body = $"Your password reset token is: {tokenWithExpirationDate.Item1}. The token will expire {tokenWithExpirationDate.Item2:u}"
            };

            await _emailService.SendEmailAsync(sendEmailDto);
        }

        [RelayCommand]
        private async Task ResetPasswordAsync()
        {
            try
            {
                ResetPasswordDto resetPasswordDto = new(Email,
                                                        Token,
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
                Closewindow(ResetPasswordWindow.Instance);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while resetting password");
                MessageBox.Show(exception.Message);
            }
            finally
            {
                Email = string.Empty;
                Token = string.Empty;
                if (ResetPasswordWindow.Instance is not null)
                {
                    ResetPasswordWindow.Instance.NewPasswordPasswordBox.Password = string.Empty; ;
                    ResetPasswordWindow.Instance.RepeatedNewPasswordPasswordBox.Password = string.Empty;
                }
            }
        }
    }
}