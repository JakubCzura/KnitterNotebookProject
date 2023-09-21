using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Exceptions.Messages;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using UserControl = System.Windows.Controls.UserControl;

namespace KnitterNotebook.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    public MainViewModel(ILogger<MainViewModel> logger,
        IMovieUrlService movieUrlService,
        ISampleService sampleService,
        IUserService userService,
        IProjectService projectService,
        IWindowContentService windowContentService,
        IThemeService themeService,
        IWebBrowserService webBrowserService,
        IProjectImageService projectImageService,
        SharedResourceViewModel sharedResourceViewModel,
        IValidator<ChangeProjectStatusDto> changeProjectStatusDtoValidator)
    {
        _logger = logger;
        _movieUrlService = movieUrlService;
        _sampleService = sampleService;
        _userService = userService;
        _projectService = projectService;
        _windowContentService = windowContentService;
        _themeService = themeService;
        _webBrowserService = webBrowserService;
        _projectImageService = projectImageService;
        _sharedResourceViewModel = sharedResourceViewModel;
        _changeProjectStatusDtoValidator = changeProjectStatusDtoValidator;
        SamplesCollectionView = CollectionViewSource.GetDefaultView(Samples);
        PlannedProjectsCollectionView = CollectionViewSource.GetDefaultView(PlannedProjects);
        ProjectsInProgressCollectionView = CollectionViewSource.GetDefaultView(ProjectsInProgress);
        FinishedProjectsCollectionView = CollectionViewSource.GetDefaultView(FinishedProjects);
        ChosenMainWindowContent = _windowContentService.ChooseMainWindowContent(MainWindowContent.SamplesUserControl);
        MovieUrlAddingViewModel.NewMovieUrlAdded += async () => MovieUrls = (await _movieUrlService.GetUserMovieUrlsAsync(User.Id)).ToObservableCollection();
        SampleAddingViewModel.NewSampleAdded += async () => Samples = (await _sampleService.GetUserSamplesAsync(User.Id)).ToObservableCollection();
        ProjectPlanningViewModel.NewProjectPlanned += async () => PlannedProjects = (await _projectService.GetUserPlannedProjectsAsync(User.Id)).ToObservableCollection();
        _sharedResourceViewModel.ProjectInProgressImageAdded += async (int projectId) => await HandleProjectInProgressImageAdded(projectId);
        _sharedResourceViewModel.UserUpdatedInDatabase += async (int userId) => await HandleUserUpdatedInDatabase(userId);
    }

    #region Properties

    private readonly ILogger<MainViewModel> _logger;
    private readonly IMovieUrlService _movieUrlService;
    private readonly ISampleService _sampleService;
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;
    private readonly IWindowContentService _windowContentService;
    private readonly IThemeService _themeService;
    private readonly IWebBrowserService _webBrowserService;
    private readonly IProjectImageService _projectImageService;
    private readonly SharedResourceViewModel _sharedResourceViewModel;
    private readonly IValidator<ChangeProjectStatusDto> _changeProjectStatusDtoValidator;

    public ICommand ShowMovieUrlAddingWindowCommand { get; } = new RelayCommand(ShowWindow<MovieUrlAddingWindow>);
    public ICommand ShowSettingsWindowCommand { get; } = new RelayCommand(ShowWindow<SettingsWindow>);
    public ICommand ShowProjectPlanningWindowCommand { get; } = new RelayCommand(ShowWindow<ProjectPlanningWindow>);
    public ICommand ShowSampleAddingWindowCommand { get; } = new RelayCommand(ShowWindow<SampleAddingWindow>);
    public ICommand ShowProjectImageAddingWindowCommand { get; } = new RelayCommand(ShowWindow<ProjectImageAddingWindow>);

    [RelayCommand]
    private void ShowPatternPdfWindow(string patternPdfPath)
    {
        //It is unnecessary to open window if path is null, but shared path should always be up to date even if the path is null
        _sharedResourceViewModel.PatternPdfPath = patternPdfPath;
        if (!string.IsNullOrWhiteSpace(patternPdfPath))
        {
            ShowWindow<PdfBrowserWindow>();
        }
    }

    public static List<string> NeedleSizeUnitList => Enum.GetNames<NeedleSizeUnit>().ToList();

    public string Greetings => $"Miło Cię widzieć {User.Nickname}!";

    [ObservableProperty]
    private UserControl _chosenMainWindowContent;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Greetings))]
    private UserDto _user = new();

    private double? _filterNeedleSize = null;

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

    private string _filterPlannedProjectName = string.Empty;

    public string FilterPlannedProjectName
    {
        get => _filterPlannedProjectName;
        set
        {
            _filterPlannedProjectName = value;
            OnPropertyChanged(nameof(FilterPlannedProjectName));
            PlannedProjectsCollectionView.Refresh();
        }
    }

    private string _filterProjectInProgressName = string.Empty;

    public string FilterProjectInProgressName
    {
        get => _filterProjectInProgressName;
        set
        {
            _filterProjectInProgressName = value;
            OnPropertyChanged(nameof(FilterProjectInProgressName));
            ProjectsInProgressCollectionView.Refresh();
        }
    }

    private string _filterFinishedProjectName = string.Empty;

    public string FilterFinishedProjectName
    {
        get => _filterFinishedProjectName;
        set
        {
            _filterFinishedProjectName = value;
            OnPropertyChanged(nameof(FilterFinishedProjectName));
            FinishedProjectsCollectionView.Refresh();
        }
    }

    [ObservableProperty]
    private ObservableCollection<MovieUrlDto> _movieUrls = new();

    [ObservableProperty]
    private MovieUrlDto? _selectedMovieUrl = null;

    private ObservableCollection<PlannedProjectDto> _plannedProjects = new();

    public ObservableCollection<PlannedProjectDto> PlannedProjects
    {
        get => _plannedProjects;
        set
        {
            _plannedProjects = value;
            OnPropertyChanged(nameof(PlannedProjects));
            PlannedProjectsCollectionView = CollectionViewSource.GetDefaultView(PlannedProjects);
            PlannedProjectsCollectionView.Filter = new Predicate<object>(x => ProjectsFilter.FilterByName<PlannedProjectDto>(x, FilterPlannedProjectName));
            OnPropertyChanged(nameof(PlannedProjectsCollectionView));
        }
    }

    private ObservableCollection<ProjectInProgressDto> _projectsInProgress = new();

    public ObservableCollection<ProjectInProgressDto> ProjectsInProgress
    {
        get => _projectsInProgress;
        set
        {
            _projectsInProgress = value;
            OnPropertyChanged(nameof(ProjectsInProgress));
            ProjectsInProgressCollectionView = CollectionViewSource.GetDefaultView(ProjectsInProgress);
            ProjectsInProgressCollectionView.Filter = new Predicate<object>(x => ProjectsFilter.FilterByName<ProjectInProgressDto>(x, FilterProjectInProgressName));
            OnPropertyChanged(nameof(ProjectsInProgressCollectionView));
        }
    }

    private ObservableCollection<FinishedProjectDto> _finishedProjects = new();

    public ObservableCollection<FinishedProjectDto> FinishedProjects
    {
        get => _finishedProjects;
        set
        {
            _finishedProjects = value;
            OnPropertyChanged(nameof(FinishedProjects));
            FinishedProjectsCollectionView = CollectionViewSource.GetDefaultView(FinishedProjects);
            FinishedProjectsCollectionView.Filter = new Predicate<object>(x => ProjectsFilter.FilterByName<FinishedProjectDto>(x, FilterFinishedProjectName));
            OnPropertyChanged(nameof(FinishedProjectsCollectionView));
        }
    }

    [ObservableProperty]
    private PlannedProjectDto? _selectedPlannedProject;

    private ProjectInProgressDto? _selectedProjectInProgress;

    public ProjectInProgressDto? SelectedProjectInProgress
    {
        get => _selectedProjectInProgress;
        set
        {
            _selectedProjectInProgress = value;
            _sharedResourceViewModel.SelectedProjectInProgressId = _selectedProjectInProgress?.Id;
            OnPropertyChanged(nameof(SelectedProjectInProgress));
        }
    }

    [ObservableProperty]
    private FinishedProjectDto? _selectedFinishedProject;

    [ObservableProperty]
    private ProjectImageDto? _selectedProjectInProgressImage;

    [ObservableProperty]
    private ProjectImageDto? _selectedFinishedProjectImage;

    [ObservableProperty]
    private SampleDto? _selectedSample;

    private ObservableCollection<SampleDto> _samples = new();

    public ObservableCollection<SampleDto> Samples
    {
        get => _samples;
        set
        {
            _samples = value;
            OnPropertyChanged(nameof(Samples));
            SamplesCollectionView = CollectionViewSource.GetDefaultView(Samples);
            SamplesCollectionView.Filter = new Predicate<object>(x => SamplesFilter.FilterByNeedleSize<SampleDto>(x, FilterNeedleSize, FilterNeedleSizeUnit));
            OnPropertyChanged(nameof(SamplesCollectionView));
        }
    }

    public ICollectionView SamplesCollectionView { get; set; }
    public ICollectionView PlannedProjectsCollectionView { get; set; }
    public ICollectionView ProjectsInProgressCollectionView { get; set; }
    public ICollectionView FinishedProjectsCollectionView { get; set; }

    #endregion Properties

    #region Methods

    [RelayCommand]
    private void ChooseMainWindowContent(MainWindowContent userControlName) => ChosenMainWindowContent = _windowContentService.ChooseMainWindowContent(userControlName);

    [RelayCommand]
    private async Task OnLoadedWindowAsync()
    {
        try
        {
            User = await _userService.GetAsync(_sharedResourceViewModel.UserId)
                ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(_sharedResourceViewModel.UserId));

            MovieUrls = (await _movieUrlService.GetUserMovieUrlsAsync(User.Id)).ToObservableCollection();
            Samples = (await _sampleService.GetUserSamplesAsync(User.Id)).ToObservableCollection();
            PlannedProjects = (await _projectService.GetUserPlannedProjectsAsync(User.Id)).ToObservableCollection();
            ProjectsInProgress = (await _projectService.GetUserProjectsInProgressAsync(User.Id)).ToObservableCollection();
            FinishedProjects = (await _projectService.GetUserFinishedProjectsAsync(User.Id)).ToObservableCollection();
            _themeService.ReplaceTheme(User.ThemeName, ApplicationTheme.Default);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while loading main window viewmodel");
            MessageBox.Show(exception.Message + "\nThe application will shut down", "Unexpected database error");
            Environment.Exit(0);
        }
    }

    [RelayCommand]
    private async Task DeleteMovieUrlAsync()
    {
        try
        {
            if (SelectedMovieUrl is not null)
            {
                await _movieUrlService.DeleteAsync(SelectedMovieUrl.Id);
                MovieUrls.Remove(SelectedMovieUrl);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while deleting movie's url");
            MessageBox.Show("Błąd podczas kasowania filmu", exception.Message);
        }
    }

    [RelayCommand]
    private async Task DeleteSampleAsync()
    {
        try
        {
            if (SelectedSample is not null)
            {
                if (!string.IsNullOrWhiteSpace(SelectedSample.ImagePath))
                {
                    _sharedResourceViewModel.FilesToDelete.Add(SelectedSample.ImagePath);
                }
                await _sampleService.DeleteAsync(SelectedSample.Id);
                Samples.Remove(SelectedSample);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while deleting sample");
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
            _logger.LogError(exception, "Error while opening movie's url in web browser");
            MessageBox.Show(exception.Message, "Błąd otworzenia filmu");
        }
    }

    [RelayCommand]
    private async Task DeletePlannedProjectAsync()
    {
        try
        {
            if (SelectedPlannedProject is not null)
            {
                await _projectService.DeleteAsync(SelectedPlannedProject.Id);
                PlannedProjects = (await _projectService.GetUserPlannedProjectsAsync(User.Id)).ToObservableCollection();
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while deleting planned project");
            MessageBox.Show(exception.Message, "Błąd skasowania planowanego projektu");
        }
    }

    [RelayCommand]
    private async Task DeleteProjectInProgressAsync()
    {
        try
        {
            if (SelectedProjectInProgress is not null)
            {
                await _projectService.DeleteAsync(SelectedProjectInProgress.Id);
                ProjectsInProgress = (await _projectService.GetUserProjectsInProgressAsync(User.Id)).ToObservableCollection();
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while deleting project in progress");
            MessageBox.Show(exception.Message, "Błąd skasowania projektu w trakcie");
        }
    }

    [RelayCommand]
    private async Task MoveProjectInProgressToPlannedProjectsAsync()
    {
        try
        {
            if (SelectedProjectInProgress is not null)
            {
                int projectId = SelectedProjectInProgress.Id;

                ChangeProjectStatusDto changeProjectStatusDto = new(projectId, ProjectStatusName.Planned);
                ValidationResult validation = await _changeProjectStatusDtoValidator.ValidateAsync(changeProjectStatusDto);
                if (!validation.IsValid)
                {
                    string errorMessage = validation.Errors.GetMessagesAsString();
                    MessageBox.Show(errorMessage, "Błąd podczas zmiany statusu projektu");
                    return;
                }

                await _projectService.ChangeProjectStatus(changeProjectStatusDto);
                ProjectsInProgress.Remove(SelectedProjectInProgress);

                PlannedProjectDto? project = await _projectService.GetPlannedProjectAsync(projectId);
                if (project is not null)
                {
                    PlannedProjects.Add(project);
                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while moving project in progress to planned projects");
            MessageBox.Show(exception.Message, "Błąd przeniesienia projektu do planowanych projektów");
        }
    }

    [RelayCommand]
    private async Task StartPlannedProjectAsync()
    {
        try
        {
            if (SelectedPlannedProject is not null)
            {
                int projectId = SelectedPlannedProject.Id;

                ChangeProjectStatusDto changeProjectStatusDto = new(projectId, ProjectStatusName.InProgress);
                ValidationResult validation = await _changeProjectStatusDtoValidator.ValidateAsync(changeProjectStatusDto);
                if (!validation.IsValid)
                {
                    string errorMessage = validation.Errors.GetMessagesAsString();
                    MessageBox.Show(errorMessage, "Błąd podczas zmiany statusu projektu");
                    return;
                }

                await _projectService.ChangeProjectStatus(changeProjectStatusDto);
                PlannedProjects.Remove(SelectedPlannedProject);

                ProjectInProgressDto? project = await _projectService.GetProjectInProgressAsync(projectId);
                if (project is not null)
                {
                    ProjectsInProgress.Add(project);
                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while starting planned project");
            MessageBox.Show(exception.Message, "Błąd rozpoczęcia planowanego projektu");
        }
    }

    [RelayCommand]
    private void LogOut()
    {
        try
        {
            _userService.LogOut();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while logging out");
        }
    }

    [RelayCommand]
    private async Task DeleteSelectedProjectInProgressImageAsync()
    {
        try
        {
            if (SelectedProjectInProgressImage is not null)
            {
                await _projectImageService.DeleteAsync(SelectedProjectInProgressImage.Id);
                if (SelectedProjectInProgress is not null)
                {
                    int projectId = SelectedProjectInProgress.Id;

                    SelectedProjectInProgress.ProjectImages = await _projectImageService.GetProjectImagesAsync(SelectedProjectInProgress.Id);
                    OnPropertyChanged(nameof(SelectedProjectInProgress));
                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while deleting project in progress image");
            MessageBox.Show(exception.Message, "Błąd skasowania zdjęcia projektu w trakcie");
        }
    }

    [RelayCommand]
    private async Task FinishProjectInProgressAsync()
    {
        try
        {
            if (SelectedProjectInProgress is not null)
            {
                int projectId = SelectedProjectInProgress.Id;

                ChangeProjectStatusDto changeProjectStatusDto = new(projectId, ProjectStatusName.Finished);
                ValidationResult validation = await _changeProjectStatusDtoValidator.ValidateAsync(changeProjectStatusDto);
                if (!validation.IsValid)
                {
                    string errorMessage = validation.Errors.GetMessagesAsString();
                    MessageBox.Show(errorMessage, "Błąd podczas zmiany statusu projektu");
                    return;
                }

                await _projectService.ChangeProjectStatus(changeProjectStatusDto);
                ProjectsInProgress.Remove(SelectedProjectInProgress);

                FinishedProjectDto? project = await _projectService.GetFinishedProjectAsync(projectId);
                if (project is not null)
                {
                    FinishedProjects.Add(project);
                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while finishing project in progress");
            MessageBox.Show(exception.Message, "Błąd zakończenia projektu w trakcie");
        }
    }

    [RelayCommand]
    private async Task MoveFinishedProjectToProjectsInProgressAsync()
    {
        try
        {
            if (SelectedFinishedProject is not null)
            {
                int projectId = SelectedFinishedProject.Id;

                ChangeProjectStatusDto changeProjectStatusDto = new(projectId, ProjectStatusName.InProgress);
                ValidationResult validation = await _changeProjectStatusDtoValidator.ValidateAsync(changeProjectStatusDto);
                if (!validation.IsValid)
                {
                    string errorMessage = validation.Errors.GetMessagesAsString();
                    MessageBox.Show(errorMessage, "Błąd podczas zmiany statusu projektu");
                    return;
                }

                await _projectService.ChangeProjectStatus(changeProjectStatusDto);
                FinishedProjects.Remove(SelectedFinishedProject);

                ProjectInProgressDto? project = await _projectService.GetProjectInProgressAsync(projectId);
                if (project is not null)
                {
                    ProjectsInProgress.Add(project);
                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while moving finished project to projects in progress");
            MessageBox.Show(exception.Message, "Błąd przeniesienia projektu do projektów w trakcie");
        }
    }

    [RelayCommand]
    private async Task DeleteFinishedProjectImageAsync()
    {
        try
        {
            if (SelectedFinishedProjectImage is not null)
            {
                await _projectImageService.DeleteAsync(SelectedFinishedProjectImage.Id);
                if (SelectedFinishedProject is not null)
                {
                    int projectId = SelectedFinishedProject.Id;

                    SelectedFinishedProject.ProjectImages = await _projectImageService.GetProjectImagesAsync(SelectedFinishedProject.Id);
                    OnPropertyChanged(nameof(SelectedFinishedProject));
                }
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while deleting finished project's image");
            MessageBox.Show(exception.Message, "Błąd skasowania zdjęcia ukończonego projektu");
        }
    }

    [RelayCommand]
    private async Task DeleteFinishedProjectAsync()
    {
        try
        {
            if (SelectedFinishedProject is not null)
            {
                await _projectService.DeleteAsync(SelectedFinishedProject.Id);
                FinishedProjects = (await _projectService.GetUserFinishedProjectsAsync(User.Id)).ToObservableCollection();
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while deleting finished project");
            MessageBox.Show(exception.Message, "Błąd skasowania ukończonego projektu");
        }
    }

    private async Task HandleProjectInProgressImageAdded(int projectId)
    {
        try
        {
            ProjectInProgressDto? project = ProjectsInProgress.FirstOrDefault(x => x.Id == projectId);
            if (project is not null)
            {
                project.ProjectImages = await _projectImageService.GetProjectImagesAsync(projectId);
            }
            OnPropertyChanged(nameof(SelectedProjectInProgress));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while fetching project in progress data");
        }
    }

    private async Task HandleUserUpdatedInDatabase(int userId)
    {
        try
        {
            User = await _userService.GetAsync(userId) ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(userId));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while fetching user's data");
        }
    }

    #endregion Methods
}