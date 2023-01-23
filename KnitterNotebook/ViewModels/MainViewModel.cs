using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.ViewModels.Helpers;
using KnitterNotebook.Views.UserControls;
using KnitterNotebook.Views.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            User = LoggedUserInformation.LoggedUser;
            ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindow);
            ShowMovieUrlAddingWindowCommand = new RelayCommand(ShowMovieUrlAddingWindow);
            MovieUrls = GetMovieUrls(User);
            MovieUrlAddingViewModel.NewMovieUrlAdded += new Action(() => MovieUrls = GetMovieUrls(User));
            SelectedMainWindowContent = new ProjectsUserControl();
            ChooseMainWindowContentCommand = new RelayCommand<string>(ChooseMainWindowContent);
        }

        #region Properties

        private UserControl selectedMainWindowContent;

        public UserControl SelectedMainWindowContent
        {
            get { return selectedMainWindowContent; }
            private set { selectedMainWindowContent = value; OnPropertyChanged(); }
        }

        public ICommand ShowSettingsWindowCommand { get; private set; }
        public ICommand ShowMovieUrlAddingWindowCommand { get; private set; }

        public ICommand ChooseMainWindowContentCommand { get; private set; }

        public string Greetings
        {
            get { return $"Miło Cię widzieć {User.Nickname}!"; }
        }

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

        private static ObservableCollection<MovieUrl> GetMovieUrls(User user)
        {
            using KnitterNotebookContext knitterNotebookContext = new();
            return new ObservableCollection<MovieUrl>(knitterNotebookContext.MovieUrls.Where(x => x.UserId == user.Id));
        }

        private void ChooseMainWindowContent(string userControlName)
        {
            try
            {
                SelectedMainWindowContent = MainWindowContent.ChooseMainWindowContent(userControlName);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd wyboru zawartości okna głównego");
                SelectedMainWindowContent = new ProjectsUserControl();
            }
        }

        #endregion Methods
    }
}