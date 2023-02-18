﻿using CommunityToolkit.Mvvm.Input;
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
    public class LoginViewModel : BaseViewModel
    {
        // public LoginViewModel(KnitterNotebookContext knitterNotebookContext)
        public LoginViewModel()
        {
            //KnitterNotebookContext = knitterNotebookContext;
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
            RegistrationWindow RegistrationWindow = new();
            RegistrationWindow.ShowDialog();
        }

        private async Task LogInAsync()
        {
            try
            {
                using (KnitterNotebookContext = new())
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

        #endregion Methods
    }
}