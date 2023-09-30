using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using FluentValidation.Results;
using KnitterNotebook.Converters;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels;

/// <summary>
/// View model for ProjectPlanningWindow.xaml
/// </summary>
public partial class ProjectPlanningViewModel : PlannedProjectBaseViewModel
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

    #region Events

    public static Action NewProjectPlanned { get; set; } = null!;

    private static void OnNewProjectPlanned() => NewProjectPlanned?.Invoke();

    #endregion Events

    #region Commands

    [RelayCommand]
    private async Task AddProjectAsync()
    {
        IEnumerable<CreateNeedleDto> needlesToCreate = NullableSizeNeedlesFilter.GetNeedlesWithPositiveSizeValue(Needle1, Needle2, Needle3, Needle4, Needle5)
                                                                                .Select(CreateNeedleDtoConverter.Convert);

        IEnumerable<CreateYarnDto> yarnsToCreate = CreateYarnDtoConverter.Convert(YarnsNamesWithDelimiter);

        try
        {
            PlanProjectDto planProjectDto = new(Name, StartDate, needlesToCreate, yarnsToCreate, Description, PatternPdfPath, _sharedResourceViewModel.UserId);
            ValidationResult validation = await _planProjectDtoValidator.ValidateAsync(planProjectDto);
            if (!validation.IsValid)
            {
                string errorMessage = validation.Errors.GetMessagesAsString();
                MessageBox.Show(errorMessage);
                return;
            }
            await _projectService.PlanProjectAsync(planProjectDto);
            OnNewProjectPlanned();
            MessageBox.Show(Translations.NewProjectPlanned);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while planning new project");
            MessageBox.Show(Translations.ErrorWhilePlanningNewProject);
        }
    }

    #endregion Commands
}