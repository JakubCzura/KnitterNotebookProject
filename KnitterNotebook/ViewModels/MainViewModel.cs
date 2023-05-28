using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Helpers;
using KnitterNotebook.Models;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Themes;
using KnitterNotebook.Views.UserControls;
using KnitterNotebook.Views.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            try
            {
                User = _databaseContext.Users
                       .Include(x => x.MovieUrls)
                       .Include(x => x.Projects)
                       .Include(x => x.Theme)
                       .Include(x => x.Samples).ThenInclude(x => x.Image)
                       .FirstOrDefault(x => x.Id == LoggedUserInformation.Id)!;
                MovieUrls = GetMovieUrls(User);
                Samples = GetSamples(User);
                if (User.Theme != null)
                {
                    string themeFullName = Path.Combine(ProjectDirectory.ProjectDirectoryFullPath, $"Themes/{User.Theme.Name}Mode.xaml");
                    ThemeChanger.SetTheme(themeFullName);
                }
                //Deleting files which paths have been already deleted from database and they are not related to logged in user
                if (User != null && User.Samples != null)
                {
                    FileHelper.DeleteUnusedUserImages(User.Samples, User.Nickname);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            SelectedSample = Samples.FirstOrDefault() ?? new Sample();
            ChoosePlannedProjectsUserControlCommand = new RelayCommand(() => ChosenMainWindowContent = new PlannedProjectsUserControl());
            ChooseProjectsInProgressUserControlCommand = new RelayCommand(() => ChosenMainWindowContent = new ProjectsInProgressUserControl());
            ChooseProjectsUserControlCommand = new RelayCommand(() => ChosenMainWindowContent = new ProjectsUserControl());
            ChooseSamplesUserControlCommand = new RelayCommand(() => ChosenMainWindowContent = new SamplesUserControl());
            ShowMovieUrlAddingWindowCommand = new RelayCommand(ShowWindow<MovieUrlAddingWindow>);
            ShowSettingsWindowCommand = new RelayCommand(ShowWindow<SettingsWindow>);
            ShowSampleAddingWindowCommand = new RelayCommand(ShowWindow<SampleAddingWindow>);
            DeleteMovieUrlCommandAsync = new AsyncRelayCommand(DeleteMovieUrlAsync);
            OpenMovieUrlInWebBrowserCommand = new RelayCommand(OpenMovieUrlInWebBrowser);
            LogOutCommand = new RelayCommand(LogOut);
            DeleteSampleCommandAsync = new AsyncRelayCommand(DeleteSampleAsync);
            MovieUrlAddingViewModel.NewMovieUrlAdded += new Action(() => MovieUrls = GetMovieUrls(User!));
        }

        #region Properties

        private readonly DatabaseContext _databaseContext;
        private readonly IMovieUrlService _movieUrlService;
        private readonly ISampleService _sampleService;
        private ObservableCollection<MovieUrl> _movieUrls = new();
        private ObservableCollection<Sample> _samples = new();
        private MovieUrl _selectedMovieUrl = new();
        private User _user = new();
        private Sample _selectedSample = new();
        private UserControl _chosenMainWindowContent = new SamplesUserControl();
        public ICommand DeleteMovieUrlCommandAsync { get; }
        public ICommand ChoosePlannedProjectsUserControlCommand { get; }
        public ICommand ChooseProjectsInProgressUserControlCommand { get; }
        public ICommand ChooseProjectsUserControlCommand { get; }
        public ICommand ChooseSamplesUserControlCommand { get; }
        public ICommand ShowMovieUrlAddingWindowCommand { get; }
        public ICommand ShowSettingsWindowCommand { get; }
        public ICommand ShowSampleAddingWindowCommand { get; }
        public ICommand OpenMovieUrlInWebBrowserCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand DeleteSampleCommandAsync { get; }

        public string Greetings => $"Miło Cię widzieć {User?.Nickname}!";

        public ObservableCollection<MovieUrl> MovieUrls
        {
            get { return _movieUrls; }
            set { _movieUrls = value; OnPropertyChanged(); }
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

        public UserControl ChosenMainWindowContent
        {
            get { return _chosenMainWindowContent; }
            set { _chosenMainWindowContent = value; OnPropertyChanged(); }
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
                if (SelectedSample != null)
                {
                    await _sampleService.DeleteAsync(SelectedSample.Id);
                    Samples = GetSamples(User);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Błąd podczas kasowania próbki obliczeniowej", exception.Message);
            }
        }

        private static ObservableCollection<Sample> GetSamples(User user) => new(user.Samples);

        private static ObservableCollection<MovieUrl> GetMovieUrls(User user) => new(user.MovieUrls);

        private void OpenMovieUrlInWebBrowser()
        {
            try
            {
                if (SelectedMovieUrl != null)
                {
                    // Open the URL in the default web browser
                    System.Diagnostics.Process.Start("cmd", "/C start" + " " + SelectedMovieUrl.Link.ToString());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd otworzenia filmu");
            }
        }

        #endregion Methods
    }
}