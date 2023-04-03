using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel(KnitterNotebookContext knitterNotebookContext)
        {
            KnitterNotebookContext = knitterNotebookContext;
            ShowMovieUrlAddingWindowCommand = new RelayCommand(ShowMovieUrlAddingWindow);
            try
            {
                User = KnitterNotebookContext.Users
                       .Include(x => x.MovieUrls)
                       .Include(x => x.Projects)
                       .Include(x => x.Theme)
                       .FirstOrDefault(x => x.Id == LoggedUserInformation.LoggedUserId)!;

                MovieUrls = new ObservableCollection<MovieUrl>(User.MovieUrls);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            SetPlannedProjectsUserControlVisibleCommand = new RelayCommand(() =>
            {
                SetUserControlsVisibilityHidden(); PlannedProjectsUserControlVisibility = Visibility.Visible;
            });
            SetProjectsInProgressUserControlVisibleCommand = new RelayCommand(() =>
            {
                SetUserControlsVisibilityHidden(); ProjectsInProgressUserControlVisibility = Visibility.Visible;
            });
            SetProjectsUserControlVisibleCommand = new RelayCommand(() =>
            {
                SetUserControlsVisibilityHidden(); ProjectsUserControlVisibility = Visibility.Visible;
            });
            SetSamplesUserControlVisibleCommand = new RelayCommand(() =>
            {
                SetUserControlsVisibilityHidden(); SamplesUserControlVisibility = Visibility.Visible;
            });
            ShowSettingsWindowCommand = new RelayCommand(ShowSettingsWindow);
            MovieUrlAddingViewModel.NewMovieUrlAdded += new Action(() => MovieUrls = GetMovieUrls(User));
            DeleteMovieUrlCommandAsync = new AsyncRelayCommand(DeleteMovieUrlAsync);
        }

        #region Properties

        public ICommand ShowSettingsWindowCommand { get; private set; }

        public ICommand ShowMovieUrlAddingWindowCommand { get; private set; }

        public ICommand DeleteMovieUrlCommandAsync { get; private set; }

        public ICommand SetProjectsUserControlVisibleCommand { get; private set; }

        public ICommand SetPlannedProjectsUserControlVisibleCommand { get; private set; }

        public ICommand SetProjectsInProgressUserControlVisibleCommand { get; private set; }

        public ICommand SetSamplesUserControlVisibleCommand { get; private set; }

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

        private Visibility projectsUserControlVisibility = Visibility.Visible;

        public Visibility ProjectsUserControlVisibility
        {
            get { return projectsUserControlVisibility; }
            set { projectsUserControlVisibility = value; OnPropertyChanged(); }
        }

        private Visibility plannedProjectsUserControlVisibility = Visibility.Hidden;

        public Visibility PlannedProjectsUserControlVisibility
        {
            get { return plannedProjectsUserControlVisibility; }
            set { plannedProjectsUserControlVisibility = value; OnPropertyChanged(); }
        }

        private Visibility projectsInProgressUserControlVisibility = Visibility.Hidden;

        public Visibility ProjectsInProgressUserControlVisibility
        {
            get { return projectsInProgressUserControlVisibility; }
            set { projectsInProgressUserControlVisibility = value; OnPropertyChanged(); }
        }

        private Visibility samplesUserControlVisibility = Visibility.Hidden;

        public Visibility SamplesUserControlVisibility
        {
            get { return samplesUserControlVisibility; }
            set { samplesUserControlVisibility = value; OnPropertyChanged(); }
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
            var movieUrlAddingWindow = App.AppHost.Services.GetService<MovieUrlAddingWindow>();
            movieUrlAddingWindow.Show();
            //var newWindowViewModel = new MovieUrlAddingViewModel(KnitterNotebookContext);
            //var newWindow = new MovieUrlAddingWindow();
            //newWindow.DataContext = newWindowViewModel;
            //newWindow.Show();
        }

        private ObservableCollection<MovieUrl> GetMovieUrls(User user)
        {
            return new ObservableCollection<MovieUrl>(KnitterNotebookContext.MovieUrls.Where(x => x.UserId == user.Id));
        }

        private async Task DeleteMovieUrlAsync()
        {
            try
            {
                if (SelectedMovieUrl != null)
                {
                    SelectedMovieUrl = await KnitterNotebookContext.MovieUrls.FirstOrDefaultAsync(x => x.Id == SelectedMovieUrl.Id);
                    KnitterNotebookContext.Remove(SelectedMovieUrl);
                    await KnitterNotebookContext.SaveChangesAsync();
                    MovieUrls = GetMovieUrls(User);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SetUserControlsVisibilityHidden()
        {
            try
            {
                ProjectsUserControlVisibility = Visibility.Hidden;
                PlannedProjectsUserControlVisibility = Visibility.Hidden;
                ProjectsInProgressUserControlVisibility = Visibility.Hidden;
                SamplesUserControlVisibility = Visibility.Hidden;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd wyboru zawartości okna głównego");
            }
        }

        #endregion Methods
    }
}