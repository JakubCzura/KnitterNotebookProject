using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Models;
using KnitterNotebook.Views.Windows;
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
            User = new();

            ShowRegistrationWindowCommand = new RelayCommand(ShowRegisterWindow);
            LoginCommand = new RelayCommand(Login);
        }

        public ICommand LoginCommand { get; }

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

        private void Login()
        {

        }
    }
}
