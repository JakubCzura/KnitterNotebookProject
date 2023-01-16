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
    public class LoginWindowViewModel : BaseViewModel
    {
        public LoginWindowViewModel()
        {
            User = null!;
            ShowRegistrationWindowCommand = new RelayCommand(ShowRegisterWindow);
            LogInCommandAsync = new AsyncRelayCommand(LogIn);
        }

        private KnitterNotebookContext KnitterNotebookContext { get; set; }
        private LoggingInManager LoggingInManager { get; set; }

        public ICommand LogInCommandAsync { get; }

        public ICommand ShowRegistrationWindowCommand { get; private set; }

        private User User { get; set; }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        private void ShowRegisterWindow()
        {
            RegistrationWindow RegistrationWindow = new();
            RegistrationWindow.ShowDialog();
        }

        private async Task LogIn()
        {
            try
            {
                using (KnitterNotebookContext = new KnitterNotebookContext())
                {
                    StandardLoggingIn standardLoggingIn = new();
                    LoggingInManager = new(standardLoggingIn, Email, LoginWindow.Instance.UserPasswordPasswordBox.Password, KnitterNotebookContext);
                    User = await LoggingInManager.LogIn();
                    if (User == null)
                    {
                        MessageBox.Show("Nieprawidłowe dane logowania");
                    }
                    else
                    {
                        LoggedUserInformation.LoggedUser = User;
                        MainWindow mainWindow = new();
                        mainWindow.Show();
                        Window.GetWindow(LoginWindow.Instance).Close();
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}