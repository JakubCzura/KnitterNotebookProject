using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Database.Login;
using KnitterNotebook.Models;
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
        public LoginViewModel(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        #region Properties

        private readonly DatabaseContext _databaseContext;

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
                StandardLoggingIn standardLoggingIn = new();
                LoggingInManager loggingInManager = new(standardLoggingIn, Email, LoginWindow.Instance.UserPasswordPasswordBox.Password, _databaseContext);
                User user = await loggingInManager.LogIn()!;
                if (user == null)
                {
                    MessageBox.Show("Nieprawidłowe dane logowania");
                    return;
                }
                LoggedUserInformation.Id = user.Id;

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