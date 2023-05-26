using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Services.Interfaces;
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
        public MainViewModel(DatabaseContext databaseContext, IMovieUrlService movieUrlService, ISampleService sampleService)
        {
            _databaseContext = databaseContext;
            _movieUrlService = movieUrlService;
            _sampleService = sampleService;
            ShowMovieUrlAddingWindowCommand = new RelayCommand(ShowMovieUrlAddingWindow);
            try
            {
                User = _databaseContext.Users
                       .Include(x => x.MovieUrls)
                       .Include(x => x.Projects)
                       .Include(x => x.Theme)
                       .Include(x => x.Samples).ThenInclude(x => x.Image)
                       .FirstOrDefault(x => x.Id == LoggedUserInformation.Id)!;
                MovieUrls = new ObservableCollection<MovieUrl>(User.MovieUrls);
                Samples = new ObservableCollection<Sample>(User.Samples);
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
            ShowSampleAddingWindowCommand = new RelayCommand(ShowSampleAddingWindow);
            MovieUrlAddingViewModel.NewMovieUrlAdded += new Action(() => MovieUrls = GetMovieUrls(User));
            DeleteMovieUrlCommandAsync = new AsyncRelayCommand(DeleteMovieUrlAsync);
            OpenMovieUrlInWebBrowserCommand = new RelayCommand(OpenMovieUrlInWebBrowser);
            LogOutCommand = new RelayCommand(LogOut);
            SelectedSample = Samples.FirstOrDefault() ?? new Sample();
            DeleteSampleCommandAsync = new AsyncRelayCommand(DeleteSampleAsync);
        }

        #region Properties

        private readonly DatabaseContext _databaseContext;
        private readonly IMovieUrlService _movieUrlService;
        private readonly ISampleService _sampleService;
        private ObservableCollection<MovieUrl> _movieUrls = new();
        private ObservableCollection<Sample> _samples = new();
        private Visibility _plannedProjectsUserControlVisibility = Visibility.Hidden;
        private Visibility _projectsInProgressUserControlVisibility = Visibility.Hidden;
        private Visibility _projectsUserControlVisibility = Visibility.Visible;
        private Visibility _samplesUserControlVisibility = Visibility.Hidden;
        private MovieUrl _selectedMovieUrl = new();
        private User _user = new();
        private Sample _selectedSample = new();

        public ICommand DeleteMovieUrlCommandAsync { get; }
        public ICommand SetPlannedProjectsUserControlVisibleCommand { get; }
        public ICommand SetProjectsInProgressUserControlVisibleCommand { get; }
        public ICommand SetProjectsUserControlVisibleCommand { get; }
        public ICommand SetSamplesUserControlVisibleCommand { get; }
        public ICommand ShowMovieUrlAddingWindowCommand { get; }
        public ICommand ShowSettingsWindowCommand { get; }
        public ICommand ShowSampleAddingWindowCommand { get; }
        public ICommand OpenMovieUrlInWebBrowserCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand DeleteSampleCommandAsync { get; }

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

        public Sample SelectedSample
        {
            get { return _selectedSample; }
            set
            {
                _selectedSample = value; OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedSampleMashesXRows));
                OnPropertyChanged(nameof(SelectedSampleNeedleSize));
            }
        }

        public ObservableCollection<Sample> Samples
        {
            get { return _samples; }
            set { _samples = value; OnPropertyChanged(); }
        }

        public string SelectedSampleMashesXRows => $"{SelectedSample?.LoopsQuantity}x{SelectedSample?.RowsQuantity}";
        public string SelectedSampleNeedleSize => $"{SelectedSample?.NeedleSize}{SelectedSample?.NeedleSizeUnit}";

        #endregion Properties

        #region Methods

        private async Task DeleteMovieUrlAsync()
        {
            try
            {
                if (SelectedMovieUrl != null)
                {
                    await _movieUrlService.DeleteAsync(SelectedMovieUrl.Id);
                    MovieUrls = GetMovieUrls(User);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Błąd podczas kasowania filmu", exception.Message);
            }
        }

        private async Task DeleteSampleAsync()
        {
            try
            {
                if(SelectedSample != null)
                {
                    await _sampleService.DeleteAsync(SelectedSample.Id);
                    Samples = GetSamples(User);
                }
            }
            catch(Exception exception) 
            {
                MessageBox.Show("Błąd podczas kasowania próbki obliczeniowej", exception.Message);
            }
        }

        private ObservableCollection<Sample> GetSamples(User user)
        {
            return new ObservableCollection<Sample>(_databaseContext.Samples.Where(x => x.User == user));
        }

        private ObservableCollection<MovieUrl> GetMovieUrls(User user)
        {
            return new ObservableCollection<MovieUrl>(_databaseContext.MovieUrls.Where(x => x.User == user));
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

        private void OpenMovieUrlInWebBrowser()
        {
            try
            {
                if (SelectedMovieUrl != null)
                {
                    // Open the URL in the default web browser
                    // System.Diagnostics.Process.Start(SelectedMovieUrl.Link.ToString());
                    //System.Diagnostics.Process.Start("http://www.google.com");
                    System.Diagnostics.Process.Start("cmd", "/C start" + " " + SelectedMovieUrl.Link.ToString());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd otworzenia filmu");
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

        private void ShowSampleAddingWindow()
        {
            ShowWindow<SampleAddingWindow>();
        }

        private void LogOut()
        {
            Environment.Exit(0);
        }

        #endregion Methods
    }
}