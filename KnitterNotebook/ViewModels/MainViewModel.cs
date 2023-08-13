using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Helpers;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        public MainViewModel(IMovieUrlService movieUrlService, ISampleService sampleService, IUserService userService, IProjectService projectService, IWindowContentService windowContentService, IThemeService themeService, IWebBrowserService webBrowserService)
        {
            _movieUrlService = movieUrlService;
            _sampleService = sampleService;
            _userService = userService;
            _projectService = projectService;
            _windowContentService = windowContentService;
            _themeService = themeService;
            _webBrowserService = webBrowserService;
            ChosenMainWindowContent = _windowContentService.ChooseMainWindowContent(MainWindowContent.SamplesUserControl);
            MovieUrlAddingViewModel.NewMovieUrlAdded += new Action(async () => MovieUrls = GetMovieUrls(await _movieUrlService.GetUserMovieUrlsAsync(User.Id)));
            SampleAddingViewModel.NewSampleAdded += new Action(async () => Samples = await _sampleService.GetUserSamplesAsync(User.Id));
            ProjectPlanningViewModel.NewProjectPlanned += new Action(async () => PlannedProjects = GetPlannedProjects((await _projectService.GetUserProjectsAsync(User.Id)).Where(x => x.ProjectStatus.Status == ProjectStatusName.Planned).ToList()));
        }

        public bool FilterByNeedleSize(object sampleToFilter, double? needleSize, NeedleSizeUnit needleSizeUnit)
            => !needleSize.HasValue ||
                sampleToFilter is Sample sample && Math.Abs(sample.NeedleSize - needleSize.Value) <= 0.0001 && sample.NeedleSizeUnit == needleSizeUnit;

        #region Properties

        private readonly IMovieUrlService _movieUrlService;
        private readonly ISampleService _sampleService;
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;
        private readonly IWindowContentService _windowContentService;
        private readonly IThemeService _themeService;
        private readonly IWebBrowserService _webBrowserService;
        public ICommand ShowMovieUrlAddingWindowCommand { get; } = new RelayCommand(ShowWindow<MovieUrlAddingWindow>);
        public ICommand ShowSettingsWindowCommand { get; } = new RelayCommand(ShowWindow<SettingsWindow>);
        public ICommand ShowProjectPlanningWindowCommand { get; } = new RelayCommand(ShowWindow<ProjectPlanningWindow>);
        public ICommand ShowSampleAddingWindowCommand { get; } = new RelayCommand(ShowWindow<SampleAddingWindow>);

        public static List<string> NeedleSizeUnitList => Enum.GetNames<NeedleSizeUnit>().ToList();

        public string Greetings => $"Miło Cię widzieć {User.Nickname}!";

        [ObservableProperty]
        private UserControl _chosenMainWindowContent;

        private double? _filterNeedleSize;

        public double? FilterNeedleSize
        {
            get => _filterNeedleSize;
            set
            {
                _filterNeedleSize = value;
                OnPropertyChanged(nameof(FilterNeedleSize));
                SamplesCollectionView.Refresh();
            }
        }

        private NeedleSizeUnit _filterNeedleSizeUnit = NeedleSizeUnit.cm;

        public NeedleSizeUnit FilterNeedleSizeUnit
        {
            get => _filterNeedleSizeUnit;
            set
            {
                _filterNeedleSizeUnit = value;
                OnPropertyChanged(nameof(FilterNeedleSizeUnit));
                SamplesCollectionView.Refresh();
            }
        }

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
        private Project? _selectedPlannedProject = null;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Greetings))]
        private UserBasicDto _user = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedSampleMashesXRows))]
        [NotifyPropertyChangedFor(nameof(SelectedSampleNeedleSize))]
        private Sample? _selectedSample = null;

        private List<Sample> _samples = new();
        public List<Sample> Samples
        {
            get => _samples;
            set
            {
                _samples = value;
                OnPropertyChanged(nameof(Samples));
                SamplesCollectionView = CollectionViewSource.GetDefaultView(Samples);
                SamplesCollectionView.Filter = new Predicate<object>(x => FilterByNeedleSize(x, FilterNeedleSize, FilterNeedleSizeUnit));
                OnPropertyChanged(nameof(SamplesCollectionView));
            }
        }

        public ICollectionView SamplesCollectionView { get; set; } = CollectionViewSource.GetDefaultView(Enumerable.Empty<Sample>());

        public ObservableCollection<Project> FilteredPlannedProjects => !string.IsNullOrWhiteSpace(FilterPlannedProjectName)
           ? new(ProjectsFilter.FilterByName(PlannedProjects, FilterPlannedProjectName))
           : PlannedProjects;

        public string SelectedSampleMashesXRows => SelectedSample is not null ? $"{SelectedSample.LoopsQuantity}x{SelectedSample.RowsQuantity}" : "";

        public string SelectedSampleNeedleSize => $"{SelectedSample?.NeedleSize}{SelectedSample?.NeedleSizeUnit}";

        public string SelectedPlannedProjectNeedles => string.Join("\n", SelectedPlannedProject?.Needles.Select(x => $"{x.Size} {x.SizeUnit}") ?? Enumerable.Empty<string>());

        public string SelectedPlannedProjectYarns => string.Join("\n", SelectedPlannedProject?.Yarns.Select(x => x.Name) ?? Enumerable.Empty<string>());

        #endregion Properties

        #region Methods

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

                MovieUrls = GetMovieUrls(await _movieUrlService.GetUserMovieUrlsAsync(User.Id));
                Samples = await _sampleService.GetUserSamplesAsync(User.Id);
                PlannedProjects = GetPlannedProjects((await _projectService.GetUserProjectsAsync(User.Id)).Where(x => x.ProjectStatus.Status == ProjectStatusName.Planned).ToList());

                _themeService.ReplaceTheme(User.ThemeName, ApplicationTheme.Default);

                //Deleting files which paths have been already deleted from database and they are not related to logged in user
                if (Samples is not null)
                {
                    FileHelper.DeleteUnusedUserImages(Samples, User.Nickname);
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
                    MovieUrls = GetMovieUrls(await _movieUrlService.GetUserMovieUrlsAsync(User.Id));
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
                    Samples = await _sampleService.GetUserSamplesAsync(User.Id);
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
                    _webBrowserService.Open(SelectedMovieUrl.Link);
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
                    PlannedProjects = GetPlannedProjects((await _projectService.GetUserProjectsAsync(User.Id)).Where(x => x.ProjectStatus.Status == ProjectStatusName.Planned).ToList());
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
                    PlannedProjects = GetPlannedProjects((await _projectService.GetUserProjectsAsync(User.Id)).Where(x => x.ProjectStatus.Status == ProjectStatusName.Planned).ToList());
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