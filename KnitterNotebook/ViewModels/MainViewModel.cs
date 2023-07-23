using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class MainViewModel : BaseViewModel
    {
        public MainViewModel(DatabaseContext databaseContext, IMovieUrlService movieUrlService, ISampleService sampleService, IUserService userService)
        {
            _databaseContext = databaseContext;
            _movieUrlService = movieUrlService;
            _sampleService = sampleService;
            SelectedSample = Samples.FirstOrDefault();
            ChoosePlannedProjectsUserControlCommand = new RelayCommand(() => ChosenMainWindowContent = new PlannedProjectsUserControl());
            ChooseProjectsInProgressUserControlCommand = new RelayCommand(() => ChosenMainWindowContent = new ProjectsInProgressUserControl());
            ChooseProjectsUserControlCommand = new RelayCommand(() => ChosenMainWindowContent = new ProjectsUserControl());
            ChooseSamplesUserControlCommand = new RelayCommand(() => ChosenMainWindowContent = new SamplesUserControl());
            MovieUrlAddingViewModel.NewMovieUrlAdded += new Action(() => MovieUrls = GetMovieUrls(User!));
            SampleAddingViewModel.NewSampleAdded += new Action(() => Samples = GetSamples(User!));
            _userService = userService;
        }

        #region Properties

        private readonly DatabaseContext _databaseContext;
        private readonly IMovieUrlService _movieUrlService;
        private readonly ISampleService _sampleService;
        private readonly IUserService _userService;

        public ICommand ChoosePlannedProjectsUserControlCommand { get; }
        public ICommand ChooseProjectsInProgressUserControlCommand { get; }
        public ICommand ChooseProjectsUserControlCommand { get; }
        public ICommand ChooseSamplesUserControlCommand { get; }
        public ICommand ShowMovieUrlAddingWindowCommand { get; } = new RelayCommand(ShowWindow<MovieUrlAddingWindow>);
        public ICommand ShowSettingsWindowCommand { get; } = new RelayCommand(ShowWindow<SettingsWindow>);
        public ICommand ShowProjectPlanningWindowCommand { get; } = new RelayCommand(ShowWindow<ProjectPlanningWindow>);
        public ICommand ShowSampleAddingWindowCommand { get; } = new RelayCommand(ShowWindow<SampleAddingWindow>);

        public static IEnumerable<string> NeedleSizeUnitList => NeedleSizeUnits.UnitsList;

        public string Greetings => $"Miło Cię widzieć {User?.Nickname}!";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FilteredSamples))]
        private double? _filterNeedleSize;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FilteredSamples))]
        private string _filterNeedleSizeUnit = NeedleSizeUnits.Units.cm.ToString();

        [ObservableProperty]
        private ObservableCollection<MovieUrl> _movieUrls = new();

        [ObservableProperty]
        private MovieUrl _selectedMovieUrl = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Greetings))]
        private User _user = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedSampleMashesXRows))]
        [NotifyPropertyChangedFor(nameof(SelectedSampleNeedleSize))]
        private Sample? _selectedSample = null;

        [ObservableProperty]
        private UserControl _chosenMainWindowContent = new SamplesUserControl();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FilteredSamples))]
        private ObservableCollection<Sample> _samples = new();

        public ObservableCollection<Sample> FilteredSamples => FilterNeedleSize > 0
            ? Samples.FilterByNeedleSize(Convert.ToDouble(FilterNeedleSize), FilterNeedleSizeUnit)
            : Samples;

        public string SelectedSampleMashesXRows => SelectedSample is not null ? $"{SelectedSample.LoopsQuantity}x{SelectedSample.RowsQuantity}" : "";

        public string SelectedSampleNeedleSize => $"{SelectedSample?.NeedleSize}{SelectedSample?.NeedleSizeUnit}";

        private static ObservableCollection<Sample> GetSamples(User user) => new(user.Samples);

        private static ObservableCollection<MovieUrl> GetMovieUrls(User user) => new(user.MovieUrls);

        #endregion Properties

        #region Methods

        [RelayCommand]
        public async Task OnLoadedWindowAsync()
        {
            try
            {
                User = await _databaseContext.Users
                          .Include(x => x.MovieUrls)
                          .Include(x => x.Projects)
                          .Include(x => x.Theme)
                          .Include(x => x.Samples).ThenInclude(x => x.Image)
                          .FirstOrDefaultAsync(x => x.Id == LoggedUserInformation.Id) ?? throw new Exception("User not found");

                MovieUrls = GetMovieUrls(User);
                Samples = GetSamples(User);
                
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
        }

        [RelayCommand]
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

        [RelayCommand]
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

        [RelayCommand]
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

        [RelayCommand]
        private void LogOut() => _userService.LogOut();
        

        #endregion Methods
    }
}