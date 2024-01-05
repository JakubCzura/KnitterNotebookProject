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
/// View model for ResetPasswordWindow.xaml
/// </summary>
public partial class ResetPasswordViewModel(ILogger<ResetPasswordViewModel> logger,
                                            IUserService userService,
                                            IValidator<ResetPasswordDto> resetPasswordDtoValidator,
                                            IEmailService emailService) : BaseViewModel
{
    private readonly ILogger<ResetPasswordViewModel> _logger = logger;
    private readonly IUserService _userService = userService;
    private readonly IValidator<ResetPasswordDto> _resetPasswordDtoValidator = resetPasswordDtoValidator;
    private readonly IEmailService _emailService = emailService;

    #region Properties

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _token = string.Empty;

    #endregion Properties

    #region Commands

    [RelayCommand]
    private async Task SendPasswordResetTokenEmailAsync()
    {
        try
        {
            bool emailExists = await _userService.IsEmailTakenAsync(Email);
            if (!emailExists)
            {
                MessageBox.Show(Translations.EmailNotFound);
                return;
            }

            (string, DateTime) tokenWithExpirationDate = await _userService.UpdatePasswordResetTokenAsync(Email);

            SendEmailDto sendEmailDto = new()
            {
                To = Email,
                Subject = "Password reset",
                Body = $"Your password reset token is: {tokenWithExpirationDate.Item1}. The token will expire {tokenWithExpirationDate.Item2:u}"
            };

            await _emailService.SendEmailAsync(sendEmailDto);
        }
        catch(Exception exception)
        {
            _logger.LogError(exception, "Error while sending email with password reset token");
            MessageBox.Show(Translations.ErrorWhileSendingEmailWithPasswordResetToken);
        }
        finally
        {
            Email = string.Empty;
        }   
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
                MessageBox.Show(errorMessage);
                return;
            }
            await _userService.ResetPasswordAsync(resetPasswordDto);
            MessageBox.Show(Translations.PasswordChangedSuccessfully);
            Closewindow(ResetPasswordWindow.Instance);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while resetting password");
            MessageBox.Show(Translations.ErrorWhileResettingPassword);
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

    #endregion Commands
}