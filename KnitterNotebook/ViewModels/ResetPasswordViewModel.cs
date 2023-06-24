using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Validators;
using KnitterNotebook.Views.UserControls;
using KnitterNotebook.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
  
    public class ResetPasswordViewModel : BaseViewModel
    {
        public ResetPasswordViewModel(IUserService userService, IValidator<ResetPasswordDto> resetPasswordDtoValidator)
        {
            ResetPasswordCommandAsync = new AsyncRelayCommand(ResetPasswordAsync);
            _userService = userService;
            _resetPasswordDtoValidator = resetPasswordDtoValidator;
        }

        private readonly IValidator<ResetPasswordDto> _resetPasswordDtoValidator;

        private readonly IUserService _userService;

        public ICommand ResetPasswordCommandAsync { get; }

        private string _emailOrNickname = string.Empty; 
        public string EmailOrNickname
        {
            get { return _emailOrNickname; }
            set { _emailOrNickname = value; OnPropertyChanged(); }
        }

        public async Task ResetPasswordAsync()
        {
            try
            {
                ResetPasswordDto resetPasswordDto = new(EmailOrNickname,
                    ResetPasswordWindow.Instance.NewPasswordPasswordBox.Password,
                    ResetPasswordWindow.Instance.RepeatedNewPasswordPasswordBox.Password);
                ValidationResult validation = await _resetPasswordDtoValidator.ValidateAsync(resetPasswordDto);
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd zmiany hasła");
                    return;
                }
                await _userService.ResetPasswordAsync(resetPasswordDto);
                MessageBox.Show($"Ustawiono nowe hasło");
                Window.GetWindow(ResetPasswordWindow.Instance).Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                EmailOrNickname = string.Empty;
                ResetPasswordWindow.Instance.NewPasswordPasswordBox.Password = string.Empty;
                ResetPasswordWindow.Instance.RepeatedNewPasswordPasswordBox.Password = string.Empty;
            }
        }
    }
}
