using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.ViewModels.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using Microsoft.EntityFrameworkCore;
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
        public MainViewModel(KnitterNotebookContext knitterNotebookContext, IMovieUrlService movieUrlService)
        {
            _knitterNotebookContext = knitterNotebookContext;
            _movieUrlService = movieUrlService;
            ShowMovieUrlAddingWindowCommand = new RelayCommand(ShowMovieUrlAddingWindow);
            try
            {
                User = _knitterNotebookContext.Users
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

        private readonly KnitterNotebookContext _knitterNotebookContext;
        private readonly IMovieUrlService _movieUrlService;
        private ObservableCollection<MovieUrl> _movieUrls = new();
        private Visibility _plannedProjectsUserControlVisibility = Visibility.Hidden;
        private Visibility _projectsInProgressUserControlVisibility = Visibility.Hidden;
        private Visibility _projectsUserControlVisibility = Visibility.Visible;
        private Visibility _samplesUserControlVisibility = Visibility.Hidden;
        private MovieUrl _selectedMovieUrl = new();
        private User _user = new();
        public ICommand DeleteMovieUrlCommandAsync { get; }
        public ICommand SetPlannedProjectsUserControlVisibleCommand { get; }
        public ICommand SetProjectsInProgressUserControlVisibleCommand { get; }
        public ICommand SetProjectsUserControlVisibleCommand { get; }
        public ICommand SetSamplesUserControlVisibleCommand { get; }
        public ICommand ShowMovieUrlAddingWindowCommand { get; }
        public ICommand ShowSettingsWindowCommand { get; }

        public string Greetings
        {
            get { return $"Miło Cię widzieć {User.Nickname}!"; }
        }

        public ObservableCollection<MovieUrl> MovieUrls
        {
            get { return _movieUrls; }
            set { _movieUrls = value; OnPropertyChanged(); }
        }

        public Visibility PlannedProjectsUserControlVisibility
        {
            get { return _plannedProjectsUserControlVisibility; }
            set { _plannedProjectsUserControlVisibility = value; OnPropertyChanged(); }
        }

        public Visibility ProjectsInProgressUserControlVisibility
        {
            get { return _projectsInProgressUserControlVisibility; }
            set { _projectsInProgressUserControlVisibility = value; OnPropertyChanged(); }
        }

        public Visibility ProjectsUserControlVisibility
        {
            get { return _projectsUserControlVisibility; }
            set { _projectsUserControlVisibility = value; OnPropertyChanged(); }
        }

        public Visibility SamplesUserControlVisibility
        {
            get { return _samplesUserControlVisibility; }
            set { _samplesUserControlVisibility = value; OnPropertyChanged(); }
        }

        public MovieUrl SelectedMovieUrl
        {
            get { return _selectedMovieUrl; }
            set { _selectedMovieUrl = value; OnPropertyChanged(); }
        }

        public User User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }

        #endregion Properties

        #region Methods

        private async Task DeleteMovieUrlAsync()
        {
            try
            {
                if (SelectedMovieUrl != null)
                {
                    await _movieUrlService.DeleteMovieUrlAsync(SelectedMovieUrl);
                    MovieUrls = GetMovieUrls(User);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Błąd podczas kasowania filmu", exception.Message);
            }
        }

        private ObservableCollection<MovieUrl> GetMovieUrls(User user)
        {
            return new ObservableCollection<MovieUrl>(_knitterNotebookContext.MovieUrls.Where(x => x.UserId == user.Id));
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

        private void ShowMovieUrlAddingWindow()
        {
            ShowWindow<MovieUrlAddingWindow>();
        }

        private void ShowSettingsWindow()
        {
            ShowWindow<SettingsWindow>();
        }

        #endregion Methods
    }
}