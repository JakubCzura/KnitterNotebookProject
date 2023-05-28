using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Database.Login;
using KnitterNotebook.Models;
using KnitterNotebook.Themes;
using KnitterNotebook.Views.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// View model for LoginWindow.xaml
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            ShowRegistrationWindowCommand = new RelayCommand(ShowWindow<RegistrationWindow>);
            LogInCommandAsync = new AsyncRelayCommand(LogInAsync);
        }

        #region Properties

        private readonly DatabaseContext _databaseContext;

        private string _email = string.Empty;

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        public ICommand LogInCommandAsync { get; }

        public ICommand ShowRegistrationWindowCommand { get; }

        #endregion Properties

        #region Methods

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
                Window.GetWindow(LoginWindow.Instance).Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion Methods
    }
}