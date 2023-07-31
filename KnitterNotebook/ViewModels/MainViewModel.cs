using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Helpers;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Themes;
using KnitterNotebook.Views.UserControls;
using KnitterNotebook.Views.Windows;
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
        public MainViewModel(IMovieUrlService movieUrlService, ISampleService sampleService, IUserService userService, IProjectService projectService, IWindowContentService windowContentService)
        {
            _movieUrlService = movieUrlService;
            _sampleService = sampleService;
            _userService = userService;
            _projectService = projectService;
            _windowContentService = windowContentService;
            SelectedSample = Samples.FirstOrDefault();
            MovieUrlAddingViewModel.NewMovieUrlAdded += new Action(() => MovieUrls = GetMovieUrls(User.MovieUrls));
            SampleAddingViewModel.NewSampleAdded += new Action(() => Samples = GetSamples(User.Samples));
        }

        #region Properties

        private readonly IMovieUrlService _movieUrlService;
        private readonly ISampleService _sampleService;
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;
        private readonly IWindowContentService _windowContentService;

        public ICommand ShowMovieUrlAddingWindowCommand { get; } = new RelayCommand(ShowWindow<MovieUrlAddingWindow>);
        public ICommand ShowSettingsWindowCommand { get; } = new RelayCommand(ShowWindow<SettingsWindow>);
        public ICommand ShowProjectPlanningWindowCommand { get; } = new RelayCommand(ShowWindow<ProjectPlanningWindow>);
        public ICommand ShowSampleAddingWindowCommand { get; } = new RelayCommand(ShowWindow<SampleAddingWindow>);

        public static IEnumerable<string> NeedleSizeUnitList => NeedleSizeUnits.UnitsList;

        public string Greetings => $"Miło Cię widzieć {User.Nickname}!";

        [ObservableProperty]
        private UserControl _chosenMainWindowContent = new SamplesUserControl();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FilteredSamples))]
        private double? _filterNeedleSize;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FilteredSamples))]
        private string _filterNeedleSizeUnit = NeedleSizeUnits.Units.cm.ToString();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FilteredPlannedProjects))]
        private string? _filterPlannedProjectName;

        [ObservableProperty]
        private ObservableCollection<MovieUrl> _movieUrls = new();

        [ObservableProperty]
        private MovieUrl _selectedMovieUrl = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FilteredPlannedProjects))]
        private ObservableCollection<Project> _plannedProjects = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedPlannedProjectNeedles))]
        [NotifyPropertyChangedFor(nameof(SelectedPlannedProjectYarns))]
        private Project _selectedPlannedProject = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Greetings))]
        private UserDto _user = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedSampleMashesXRows))]
        [NotifyPropertyChangedFor(nameof(SelectedSampleNeedleSize))]
        private Sample? _selectedSample = null;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FilteredSamples))]
        private ObservableCollection<Sample> _samples = new();

        public ObservableCollection<Sample> FilteredSamples => FilterNeedleSize > 0
            ? new(Samples.FilterByNeedleSize(Convert.ToDouble(FilterNeedleSize), FilterNeedleSizeUnit))
            : Samples;

        public ObservableCollection<Project> FilteredPlannedProjects => !string.IsNullOrWhiteSpace(FilterPlannedProjectName)
           ? new(PlannedProjects.FilterByName(FilterPlannedProjectName))
           : PlannedProjects;

        public string SelectedSampleMashesXRows => SelectedSample is not null ? $"{SelectedSample.LoopsQuantity}x{SelectedSample.RowsQuantity}" : "";

        public string SelectedSampleNeedleSize => $"{SelectedSample?.NeedleSize}{SelectedSample?.NeedleSizeUnit}";

        public string SelectedPlannedProjectNeedles => string.Join("\n", SelectedPlannedProject?.Needles.Select(x => $"{x.Size} {x.SizeUnit}") ?? Enumerable.Empty<string>());
        
        public string SelectedPlannedProjectYarns => string.Join("\n", SelectedPlannedProject?.Yarns.Select(x => x.Name) ?? Enumerable.Empty<string>());

        #endregion Properties

        #region Methods
        private static ObservableCollection<Sample> GetSamples(List<Sample> samples) => new(samples);

        private static ObservableCollection<MovieUrl> GetMovieUrls(List<MovieUrl> movieUrls) => new(movieUrls);

        private static ObservableCollection<Project> GetPlannedProjects(List<Project> projects) => new(projects);

        [RelayCommand]
        private void ChooseMainWindowContent(MainWindowContent userControlName) => ChosenMainWindowContent = _windowContentService.ChooseMainWindowContent(userControlName);

        [RelayCommand]
        public async Task OnLoadedWindowAsync()
        {
            try
            {
                User = await _userService.GetAsync(LoggedUserInformation.Id)
                       ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(LoggedUserInformation.Id));

                MovieUrls = GetMovieUrls(User.MovieUrls);
                Samples = GetSamples(User.Samples);
                PlannedProjects = GetPlannedProjects(User.Projects.Where(x => x.ProjectStatusId == 1).ToList());

                if (User.Theme is not null)
                {
                    string themeFullPath = Paths.ThemeFullPath(User.Theme.Name);
                    ThemeChanger.SetTheme(themeFullPath);
                }
                //Deleting files which paths have been already deleted from database and they are not related to logged in user
                if (User.Samples is not null)
                {
                    FileHelper.DeleteUnusedUserImages(User.Samples, User.Nickname);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\nThe application will shut down", "Unexpected database error");
                Environment.Exit(0);
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
                    MovieUrls = GetMovieUrls(User.MovieUrls);
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
                    Samples = GetSamples(User.Samples);
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
        private async Task DeletePlannedProjectAsync()
        {
            try
            {
                if (SelectedPlannedProject?.Id > 0)
                {
                    await _projectService.DeleteAsync(SelectedPlannedProject.Id);
                    PlannedProjects = GetPlannedProjects(User.Projects.Where(x => x.ProjectStatusId == 1).ToList());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd skasowania planowanego projektu");
            }
        }

        [RelayCommand]
        private async Task StartPlannedProjectAsync()
        {
            try
            {
                if (SelectedPlannedProject?.Id > 0)
                {
                    SelectedPlannedProject.ProjectStatusId = 2;
                    await _projectService.UpdateAsync(SelectedPlannedProject);
                    PlannedProjects = GetPlannedProjects(User.Projects.Where(x => x.ProjectStatusId == 1).ToList());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd rozpoczęcia planowanego projektu");
            }
        }

        [RelayCommand]
        private void LogOut() => _userService.LogOut();

        #endregion Methods
    }
}