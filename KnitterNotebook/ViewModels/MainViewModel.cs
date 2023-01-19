using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Views.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            KnitterNotebookContext = new();
            User = LoggedUserInformation.LoggedUser;
            ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindow);
            ShowMovieUrlAddingWindowCommand = new RelayCommand(ShowMovieUrlAddingWindow);
            MovieUrls = GetMovieUrls(User, KnitterNotebookContext);
            MovieUrlAddingViewModel.NewMovieUrlAdded += RefreshMovieUrls;
        }

        #region Properties

        public ICommand ShowSettingsWindowCommand { get; private set; }
        public ICommand ShowMovieUrlAddingWindowCommand { get; private set; }

        private KnitterNotebookContext KnitterNotebookContext { get; set; }

        private User user;

        public User User
        {
            get { return user; }
            set { user = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MovieUrl> movieUrls;

        public ObservableCollection<MovieUrl> MovieUrls
        {
            get { return movieUrls; }
            set { movieUrls = value; OnPropertyChanged(); }
        }
        #endregion Properties

        #region Methods

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

        private static ObservableCollection<MovieUrl> GetMovieUrls(User user, KnitterNotebookContext knitterNotebookContext)
        {
            return new ObservableCollection<MovieUrl>(knitterNotebookContext.MovieUrls.Where(x => x.UserId == user.Id));
        }

        private void RefreshMovieUrls()
        {
            MovieUrls = GetMovieUrls(User, KnitterNotebookContext);
        }

        #endregion Methods
    }
}