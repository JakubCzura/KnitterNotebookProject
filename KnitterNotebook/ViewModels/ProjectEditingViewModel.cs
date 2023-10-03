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
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels;

/// <summary>
/// View model for ProjectEditingWindow.xaml
/// </summary>
public partial class ProjectEditingViewModel : PlannedProjectBaseViewModel
{
    public ProjectEditingViewModel(ILogger<ProjectEditingViewModel> logger,
        IProjectService projectService,
        IValidator<EditProjectDto> editPlannedProjectDtoValidator,
        SharedResourceViewModel sharedResourceViewModel)
    {
        _logger = logger;
        _projectService = projectService;
        _editPlannedProjectDtoValidator = editPlannedProjectDtoValidator;
        _sharedResourceViewModel = sharedResourceViewModel;
    }

    private readonly ILogger<ProjectEditingViewModel> _logger;
    private readonly IProjectService _projectService;
    private readonly IValidator<EditProjectDto> _editPlannedProjectDtoValidator;
    private readonly SharedResourceViewModel _sharedResourceViewModel;

    private string? _originalPatternPdfPath = null;

    #region Properties

    [ObservableProperty]
    private int _needle1SizeUnitIndex = 0;

    [ObservableProperty]
    private int _needle2SizeUnitIndex = 0;

    [ObservableProperty]
    private int _needle3SizeUnitIndex = 0;

    [ObservableProperty]
    private int _needle4SizeUnitIndex = 0;

    [ObservableProperty]
    private int _needle5SizeUnitIndex = 0;

    #endregion Properties

    #region Commands

    [RelayCommand]
    private async Task OnLoadedWindowAsync()
    {
        try
        {
            //User can edit planned project and project in progress
            //Their edition is identical, so we can use the same method to get data of choosen project
            PlannedProjectDto? plannedProjectDto = await _projectService.GetPlannedProjectAsync(_sharedResourceViewModel.EditedProjectIdAndStatus.Item1);
            if (plannedProjectDto == null)
            {
                MessageBox.Show(Translations.ErrorWhileGettingProjectData);
                Closewindow(ProjectEditingWindow.Instance);
                return;
            }

            _originalPatternPdfPath = plannedProjectDto.PatternPdfPath;

            Name = plannedProjectDto.Name;
            StartDate = plannedProjectDto.StartDate;
            YarnsNamesWithDelimiter = YarnsNamesWithDelimiterConverter.Convert(plannedProjectDto.Yarns);
            Description = plannedProjectDto.Description;
            PatternPdfPath = plannedProjectDto.PatternPdfPath;
            List<NeedleDto> needles = plannedProjectDto.Needles;
            if (needles.Count >= 1)
            {
                Needle1SizeUnitIndex = InitializeNeedle(Needle1, needles[0]);
                OnPropertyChanged(nameof(Needle1));
            }
            if (needles.Count >= 2)
            {
                Needle2SizeUnitIndex = InitializeNeedle(Needle2, needles[1]);
                OnPropertyChanged(nameof(Needle2));
            }
            if (needles.Count >= 3)
            {
                Needle3SizeUnitIndex = InitializeNeedle(Needle3, needles[2]);
                OnPropertyChanged(nameof(Needle3));
            }
            if (needles.Count >= 4)
            {
                Needle4SizeUnitIndex = InitializeNeedle(Needle4, needles[3]);
                OnPropertyChanged(nameof(Needle4));
            }
            if (needles.Count == 5)
            {
                Needle5SizeUnitIndex = InitializeNeedle(Needle5, needles[4]);
                OnPropertyChanged(nameof(Needle5));
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception while getting project's data");
            MessageBox.Show(Translations.ErrorWhileGettingProjectData);
            Closewindow(ProjectEditingWindow.Instance);
        }
    }

    [RelayCommand]
    private async Task EditProjectAsync()
    {
        IEnumerable<CreateNeedleDto> needlesToEdit = NullableSizeNeedlesFilter.GetNeedlesWithPositiveSizeValue(Needle1, Needle2, Needle3, Needle4, Needle5)
                                                                                .Select(CreateNeedleDtoConverter.Convert);

        IEnumerable<CreateYarnDto> yarnsToEdit = CreateYarnDtoConverter.Convert(YarnsNamesWithDelimiter);

        try
        {
            EditProjectDto editPlannedProjectDto = new(_sharedResourceViewModel.EditedProjectIdAndStatus.Item1, Name, StartDate, needlesToEdit, yarnsToEdit, Description, PatternPdfPath, _sharedResourceViewModel.UserId);
            ValidationResult validation = await _editPlannedProjectDtoValidator.ValidateAsync(editPlannedProjectDto);
            if (!validation.IsValid)
            {
                string errorMessage = validation.Errors.GetMessagesAsString();
                MessageBox.Show(errorMessage);
                return;
            }
            //if user changes pattern pdf, then old one should be deleted
            if (!string.IsNullOrEmpty(_originalPatternPdfPath) && _originalPatternPdfPath != PatternPdfPath)
            {
                _sharedResourceViewModel.FilesToDelete.Add(_originalPatternPdfPath);
            }
            await _projectService.EditProjectAsync(editPlannedProjectDto);
            _sharedResourceViewModel.OnProjectEdited();
            MessageBox.Show(Translations.ProjectEditedSuccessfully);
            Closewindow(ProjectEditingWindow.Instance);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while editing project");
            MessageBox.Show(Translations.ErrorWhileEditingProject);
        }
    }

    #endregion Commands

    #region Methods

    private static int InitializeNeedle(NullableSizeNeedle nullableSizeNeedle, NeedleDto needleDto)
    {
        nullableSizeNeedle.Size = needleDto.Size;
        nullableSizeNeedle.SizeUnit = needleDto.SizeUnit;
        return nullableSizeNeedle.SizeUnit == NeedleSizeUnit.mm ? 0 : 1;
    }

    #endregion Methods
}