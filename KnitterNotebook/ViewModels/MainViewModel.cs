using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Views.UserControls;
using KnitterNotebook.Views.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            try
            {
                using (KnitterNotebookContext = new())
                {
                    User = KnitterNotebookContext.Users
                           .Include(x => x.MovieUrls)
                           .Include(x => x.Projects)
                           .Include(x => x.Theme)
                           .FirstOrDefault(x => x.Id == LoggedUserInformation.LoggedUserId)!;
                }
                MovieUrls = new ObservableCollection<MovieUrl>(User.MovieUrls);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindow);
            ShowMovieUrlAddingWindowCommand = new RelayCommand(ShowMovieUrlAddingWindow);
            MovieUrlAddingViewModel.NewMovieUrlAdded += new Action(() => MovieUrls = GetMovieUrls(User));
            ChooseMainWindowContentCommand = new RelayCommand<Type>(ChooseMainWindowContent!);
            DeleteMovieUrlCommandAsync = new AsyncRelayCommand(DeleteMovieUrlAsync);
        }

        #region Properties       

        public ICommand ShowSettingsWindowCommand { get; private set; }

        public ICommand ShowMovieUrlAddingWindowCommand { get; private set; }

        public ICommand ChooseMainWindowContentCommand { get; private set; }

        public ICommand DeleteMovieUrlCommandAsync { get; private set; }

        private KnitterNotebookContext KnitterNotebookContext { get; set; }

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

        private MovieUrl selectedMovieUrl;

        public MovieUrl SelectedMovieUrl
        {
            get { return selectedMovieUrl; }
            set { selectedMovieUrl = value; OnPropertyChanged(); }
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

        private ObservableCollection<MovieUrl> GetMovieUrls(User user)
        {
            using (KnitterNotebookContext = new KnitterNotebookContext())
            {
                return new ObservableCollection<MovieUrl>(KnitterNotebookContext.MovieUrls.Where(x => x.UserId == user.Id));
            }
        }

        private void ChooseMainWindowContent(Type userControl)
        {
            try
            {
               
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd wyboru zawartości okna głównego");
            }
        }


        private async Task DeleteMovieUrlAsync()
        {
            try
            {
                if (SelectedMovieUrl != null)
                {
                    using (KnitterNotebookContext = new())
                    {
                        SelectedMovieUrl = await KnitterNotebookContext.MovieUrls.FirstOrDefaultAsync(x => x.Id == SelectedMovieUrl.Id);
                        KnitterNotebookContext.Remove(SelectedMovieUrl);

                        await KnitterNotebookContext.SaveChangesAsync();
                    }
                    MovieUrls = GetMovieUrls(User);
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