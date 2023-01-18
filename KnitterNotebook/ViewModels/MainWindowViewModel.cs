using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Views.Windows;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            User = LoggedUserInformation.LoggedUser;
            ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindow);
            ShowMovieUrlAddingWindowCommand = new RelayCommand(ShowMovieUrlAddingWindow);
        }

        public ICommand ShowSettingsWindowCommand { get; private set; }
        public ICommand ShowMovieUrlAddingWindowCommand { get; private set; }

        private User user;

        public User User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(); }
        }

        private void ShowSettingsWindow()
        {
            SettingsWindow settingsWindow = new();
            settingsWindow.ShowDialog();
        }

        private void ShowMovieUrlAddingWindow()
        {
            MovieUrlAddingWindow movieUrlAddingWindow = new();
            movieUrlAddingWindow.Show();
        }
    }
}