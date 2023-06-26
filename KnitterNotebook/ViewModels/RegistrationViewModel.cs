using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        public RegistrationViewModel(IUserService userService, IValidator<RegisterUserDto> registerUserDtoValidator)
        {
            _userService = userService;
            _registerUserDtoValidator = registerUserDtoValidator;
            RegisterUserCommandAsync = new AsyncRelayCommand(RegisterUser);
        }

        #region Properties

        private readonly IUserService _userService;
        private readonly IValidator<RegisterUserDto> _registerUserDtoValidator;
        private string _email = string.Empty;
        private string _nickname = string.Empty;

        public string Email
        {
            get =>_email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Nickname
        {
            get => _nickname;
            set { _nickname = value; OnPropertyChanged(); }
        }

        public ICommand RegisterUserCommandAsync { get; }

        #endregion Properties

        #region Methods

        private async Task RegisterUser()
        {
            try
            {
                RegisterUserDto registerUserDto = new(Nickname, Email, RegistrationWindow.Instance.UserPasswordPasswordBox.Password);
                var validation = _registerUserDtoValidator.Validate(registerUserDto);
                if (!validation.IsValid)
                {
                    string errorMessage = string.Join(Environment.NewLine, validation.Errors.Select(x => x.ErrorMessage));
                    MessageBox.Show(errorMessage, "Błąd podczas rejestracji", MessageBoxButton.OK);
                    return;
                }
                await _userService.CreateAsync(registerUserDto);
                Window.GetWindow(RegistrationWindow.Instance).Close();
                MessageBox.Show("Rejestracja przebiegła pomyślnie");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion Methods
    }
}