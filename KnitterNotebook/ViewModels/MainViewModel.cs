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
using System.Collections.Generic;
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
                FilteredSamples = Samples;
                if (User?.Theme is not null)
                {
                    string themeFullPath = Paths.ThemeFullPath(User.Theme.Name);
                    ThemeChanger.SetTheme(themeFullPath);
                }
                //Deleting files which paths have been already deleted from database and they are not related to logged in user
                if (User?.Samples is not null)
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
            SampleAddingViewModel.NewSampleAdded += new Action(() => Samples = GetSamples(User!));
        }

        #region Properties

        private readonly DatabaseContext _databaseContext;
        private readonly IMovieUrlService _movieUrlService;
        private readonly ISampleService _sampleService;
        private ObservableCollection<MovieUrl> _movieUrls = new();
        private ObservableCollection<Sample> _samples = new();
        private ObservableCollection<Sample> _filteredSamples = new();
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

        public static IEnumerable<string> NeedleSizeUnits => new[] { "mm", "cm" };
        public string Greetings => $"Miło Cię widzieć {User?.Nickname}!";

        private double? _filterNeedleSize;

        public double? FilterNeedleSize
        {
            get => _filterNeedleSize;
            set
            {
                _filterNeedleSize = value; OnPropertyChanged();
                if (value > 0)
                {
                    FilteredSamples = SamplesFilter.FilterByNeedleSize(Samples, Convert.ToDouble(value), FilterNeedleSizeUnit);
                }
                else
                {
                    FilteredSamples = Samples;
                }
            }
        }

        private string _filterNeedleSizeUnit = "mm";

        public string FilterNeedleSizeUnit
        {
            get => _filterNeedleSizeUnit;
            set
            {
                _filterNeedleSizeUnit = value; OnPropertyChanged();
                if (FilterNeedleSize > 0)
                {
                    FilteredSamples = SamplesFilter.FilterByNeedleSize(Samples, Convert.ToDouble(FilterNeedleSize), value);
                }
                else
                {
                    FilteredSamples = Samples;
                }
            }
        }       

        public ObservableCollection<MovieUrl> MovieUrls
        {
            get => _movieUrls;
            set { _movieUrls = value; OnPropertyChanged(); }
        }

        public MovieUrl SelectedMovieUrl
        {
            get => _selectedMovieUrl;
            set { _selectedMovieUrl = value; OnPropertyChanged(); }
        }

        public User User
        {
            get => _user;
            set { _user = value; OnPropertyChanged(); }
        }

        public Sample SelectedSample
        {
            get => _selectedSample;
            set
            {
                _selectedSample = value; OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedSampleMashesXRows));
                OnPropertyChanged(nameof(SelectedSampleNeedleSize));
            }
        }

        public UserControl ChosenMainWindowContent
        {
            get => _chosenMainWindowContent;
            set { _chosenMainWindowContent = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Sample> Samples
        {
            get => _samples;
            set
            {
                _samples = value; OnPropertyChanged();
                if (FilterNeedleSize > 0)
                {
                    FilteredSamples = SamplesFilter.FilterByNeedleSize(value, Convert.ToDouble(FilterNeedleSize), FilterNeedleSizeUnit);
                }
                else
                {
                    FilteredSamples = value;
                }
            }
        }

        public ObservableCollection<Sample> FilteredSamples
        {
            get => _filteredSamples;
            set { _filteredSamples = value; OnPropertyChanged(); }
        }

        public string SelectedSampleMashesXRows => $"{SelectedSample?.LoopsQuantity}x{SelectedSample?.RowsQuantity}";
        public string SelectedSampleNeedleSize => $"{SelectedSample?.NeedleSize}{SelectedSample?.NeedleSizeUnit}";
        private static ObservableCollection<Sample> GetSamples(User user) => new(user.Samples);
        private static ObservableCollection<MovieUrl> GetMovieUrls(User user) => new(user.MovieUrls);

        #endregion Properties

        #region Methods

        private async Task DeleteMovieUrlAsync()
        {
            try
            {
                if (SelectedMovieUrl?.Id > 0)
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
                if (SelectedSample?.Id > 0)
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

       

        private void OpenMovieUrlInWebBrowser()
        {
            try
            {
                if (SelectedMovieUrl?.Link is not null)
                {
                    UrlOpener.OpenInWebBrowser(SelectedMovieUrl.Link.ToString());
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