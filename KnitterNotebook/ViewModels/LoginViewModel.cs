using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// View model for LoginWindow.xaml
    /// </summary>
    public partial class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(IUserService userService, IValidator<LogInDto> logInDtoValidator)
        {
            _userService = userService;
            _logInDtoValidator = logInDtoValidator;
        }

        private readonly IUserService _userService;
        private readonly IValidator<LogInDto> _logInDtoValidator;

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
                    MessageBox.Show("Nieprawidłowe dane logowania");
                    return;
                }

                int? userId = await _userService.LogInAsync(logInDto);
                if (!userId.HasValue)
                {
                    MessageBox.Show("Nie odnaleziono użytkownika");
                    return;
                }

                LoggedUserInformation.Id = userId.Value;

                ShowWindow<MainWindow>();
                Closewindow(LoginWindow.Instance);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion Methods
    }
}