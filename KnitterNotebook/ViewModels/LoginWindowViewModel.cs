using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database.Registration;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Validators;
using KnitterNotebook.Views.Windows;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using KnitterNotebook.Database.Login;

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

        private User user;

        public User User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(nameof(User)); }
        }

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
