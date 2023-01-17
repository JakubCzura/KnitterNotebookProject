using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Views.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            User = LoggedUserInformation.LoggedUser;
            ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindow);
        }

        public ICommand ShowSettingsWindowCommand { get; private set; }

        private User user;

        public User User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(); }
        }

        private void ShowSettingsWindow()
        {
            SettingsWindow SettingsWindow = new();
            SettingsWindow.ShowDialog();
        }
    }
}