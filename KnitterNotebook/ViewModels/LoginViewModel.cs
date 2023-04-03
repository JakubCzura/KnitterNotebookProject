using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Database.Login;
using KnitterNotebook.Models;
using KnitterNotebook.Themes;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
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
        public LoginViewModel(KnitterNotebookContext knitterNotebookContext)
        {
            KnitterNotebookContext = knitterNotebookContext;
            ShowRegistrationWindowCommand = new RelayCommand(ShowRegisterWindow);
            LogInCommandAsync = new AsyncRelayCommand(LogInAsync);
        }

        #region Properties

        private KnitterNotebookContext KnitterNotebookContext { get; set; }
        private LoggingInManager LoggingInManager { get; set; }

        public ICommand LogInCommandAsync { get; }

        public ICommand ShowRegistrationWindowCommand { get; private set; }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        #endregion Properties

        #region Methods

        private void ShowRegisterWindow()
        {
            var registrationWindow = App.AppHost.Services.GetService<RegistrationWindow>();
            registrationWindow.ShowDialog();
        }

        private async Task LogInAsync()
        {
            try
            {
                StandardLoggingIn standardLoggingIn = new();
                LoggingInManager = new(standardLoggingIn, Email, LoginWindow.Instance.UserPasswordPasswordBox.Password, KnitterNotebookContext);
                User user = await LoggingInManager.LogIn()!;
                if (user == null)
                {
                    MessageBox.Show("Nieprawidłowe dane logowania");
                }
                else
                {
                    LoggedUserInformation.LoggedUserId = user.Id;
                    Theme theme = KnitterNotebookContext.Themes.FirstOrDefault(x => x.Id == user.ThemeId);
                    string themeFullName = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, $"Themes/{theme.Name}Mode.xaml");
                    ThemeChanger.SetTheme(themeFullName);

                    var startupWindow = App.AppHost.Services.GetService<MainWindow>();
                    startupWindow.Show();

                    Window.GetWindow(LoginWindow.Instance).Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion Methods
    }
}