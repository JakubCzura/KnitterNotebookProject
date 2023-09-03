using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Converters;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels
{
    public partial class ProjectPlanningViewModel : ObservableObject
    {
        public ProjectPlanningViewModel(ILogger<ProjectPlanningViewModel> logger,
            IProjectService projectService,
            IValidator<PlanProjectDto> planProjectDtoValidator,
            SharedResourceViewModel sharedResourceViewModel)
        {
            _logger = logger;
            _projectService = projectService;
            _planProjectDtoValidator = planProjectDtoValidator;
            _sharedResourceViewModel = sharedResourceViewModel;
        }

        private readonly ILogger<ProjectPlanningViewModel> _logger;
        private readonly IProjectService _projectService;
        private readonly IValidator<PlanProjectDto> _planProjectDtoValidator;
        private readonly SharedResourceViewModel _sharedResourceViewModel;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private DateTime? _startDate = null;

        [ObservableProperty]
        private string _yarnsNamesWithDelimiter = string.Empty;

        //Tzw. "inne"
        [ObservableProperty]
        private string? _description = null;

        [ObservableProperty]
        private string? _patternPdfPath = null;

        [ObservableProperty]
        private NullableSizeNeedle _needle1 = new();

        [ObservableProperty]
        private NullableSizeNeedle _needle2 = new();

        [ObservableProperty]
        private NullableSizeNeedle _needle3 = new();

        [ObservableProperty]
        private NullableSizeNeedle _needle4 = new();

        [ObservableProperty]
        private NullableSizeNeedle _needle5 = new();

        public static IEnumerable<string> NeedleSizeUnitList => Enum.GetNames<NeedleSizeUnit>();

        public static Action NewProjectPlanned { get; set; } = null!;

        private static void OnNewProjectPlanned() => NewProjectPlanned?.Invoke();

        [RelayCommand]
        private void ChoosePatternPdf()
        {
            OpenFileDialog dialog = new()
            {
                Filter = FileDialogFilter.PdfFilter
            };
            dialog.ShowDialog();
            PatternPdfPath = dialog.FileName;
        }

        [RelayCommand]
        private void DeletePatternPdf() => PatternPdfPath = null;

        [RelayCommand]
        private async Task AddProjectAsync()
        {
            IEnumerable<CreateNeedleDto> needlesToCreate = NullableSizeNeedlesFilter.GetNeedlesWithPositiveSizeValue(Needle1, Needle2, Needle3, Needle4, Needle5)
                                                                                    .Select(CreateNeedleDtoConverter.Convert);

            IEnumerable<CreateYarnDto> yarnsToCreate = !string.IsNullOrWhiteSpace(YarnsNamesWithDelimiter)
                                                    ? CreateYarnDtoConverter.Convert(YarnsNamesWithDelimiter)
                                                    : Enumerable.Empty<CreateYarnDto>();

            try
            {
                PlanProjectDto planProjectDto = new(Name, StartDate, needlesToCreate, yarnsToCreate, Description, PatternPdfPath, _sharedResourceViewModel.UserId);
                ValidationResult validation = await _planProjectDtoValidator.ValidateAsync(planProjectDto);
                if (!validation.IsValid)
                {
                    string errorMessage = validation.Errors.GetMessagesAsString();
                    MessageBox.Show(errorMessage, "Błąd podczas planowania projektu");
                    return;
                }
                await _projectService.PlanProjectAsync(planProjectDto);
                OnNewProjectPlanned();
                MessageBox.Show("Zaplanowano nowy projekt");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error while planning new project");
                MessageBox.Show(exception.Message);
            }
        }
    }
}