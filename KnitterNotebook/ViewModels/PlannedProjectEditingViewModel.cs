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
using KnitterNotebook.Validators;
using KnitterNotebook.Views.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KnitterNotebook.ViewModels
{
    public partial class PlannedProjectEditingViewModel : BaseViewModel
    {
        public PlannedProjectEditingViewModel(ILogger<PlannedProjectEditingViewModel> logger, 
            IProjectService projectService, 
            IValidator<EditPlannedProjectDto> editPlannedProjectDtoValidator, 
            SharedResourceViewModel sharedResourceViewModel)
        {
            _logger = logger;
            _projectService = projectService;
            _editPlannedProjectDtoValidator = editPlannedProjectDtoValidator;
            _sharedResourceViewModel = sharedResourceViewModel;
        }

        private readonly ILogger<PlannedProjectEditingViewModel> _logger;
        private readonly IProjectService _projectService;
        private readonly IValidator<EditPlannedProjectDto> _editPlannedProjectDtoValidator;
        private readonly SharedResourceViewModel _sharedResourceViewModel;

        private string? _originalPatternPdfPath = null;

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
        private int _needle1SizeUnitIndex = 0;

        [ObservableProperty]
        private NullableSizeNeedle _needle2 = new();

        [ObservableProperty]
        private int _needle2SizeUnitIndex = 0;

        [ObservableProperty]
        private NullableSizeNeedle _needle3 = new();

        [ObservableProperty]
        private int _needle3SizeUnitIndex = 0;

        [ObservableProperty]
        private NullableSizeNeedle _needle4 = new();

        [ObservableProperty]
        private int _needle4SizeUnitIndex = 0;

        [ObservableProperty]
        private NullableSizeNeedle _needle5 = new();

        [ObservableProperty]
        private int _needle5SizeUnitIndex = 0;

        public static IEnumerable<string> NeedleSizeUnitList => Enum.GetNames<NeedleSizeUnit>();

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
        private async Task OnLoadedWindowAsync()
        {
            try
            {
                PlannedProjectDto? plannedProjectDto = await _projectService.GetPlannedProjectAsync(_sharedResourceViewModel.EditPlannedProjectId);
                if (plannedProjectDto == null)
                {
                    MessageBox.Show("Exception while getting project's data - null planned project");
                    Closewindow(PlannedProjectEditingWindow.Instance);
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
                    Needle1.Size = needles[0].Size;
                    Needle1.SizeUnit = needles[0].SizeUnit;
                    if (Needle1.SizeUnit == NeedleSizeUnit.cm)
                    {
                        Needle1SizeUnitIndex = 1;
                    }
                    OnPropertyChanged(nameof(Needle1));
                }
                if (needles.Count >= 2)
                {
                    Needle2.Size = needles[1].Size;
                    Needle2.SizeUnit = needles[1].SizeUnit;
                    if (Needle2.SizeUnit == NeedleSizeUnit.cm)
                    {
                        Needle2SizeUnitIndex = 1;
                    }
                    OnPropertyChanged(nameof(Needle2));
                }
                if (needles.Count >= 3)
                {
                    Needle3.Size = needles[2].Size;
                    Needle3.SizeUnit = needles[2].SizeUnit;
                    if (Needle3.SizeUnit == NeedleSizeUnit.cm)
                    {
                        Needle3SizeUnitIndex = 1;
                    }
                    OnPropertyChanged(nameof(Needle3));
                }
                if (needles.Count >= 4)
                {
                    Needle4.Size = needles[3].Size;
                    Needle4.SizeUnit = needles[3].SizeUnit;
                    if (Needle4.SizeUnit == NeedleSizeUnit.cm)
                    {
                        Needle4SizeUnitIndex = 1;
                    }
                    OnPropertyChanged(nameof(Needle4));
                }
                if (needles.Count == 5)
                {
                    Needle5.Size = needles[4].Size;
                    Needle5.SizeUnit = needles[4].SizeUnit;
                    if (Needle5.SizeUnit == NeedleSizeUnit.cm)
                    {
                        Needle5SizeUnitIndex = 1;
                    }
                    OnPropertyChanged(nameof(Needle5));
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception while getting project's data");
                MessageBox.Show("Exception while getting project's data");
                Closewindow(PlannedProjectEditingWindow.Instance);
            }
        }

        [RelayCommand]
        private async Task EditProjectAsync()
        {
            IEnumerable<CreateNeedleDto> needlesToCreate = NullableSizeNeedlesFilter.GetNeedlesWithPositiveSizeValue(Needle1, Needle2, Needle3, Needle4, Needle5)
                                                                                .Select(CreateNeedleDtoConverter.Convert);

            IEnumerable<CreateYarnDto> yarnsToCreate = CreateYarnDtoConverter.Convert(YarnsNamesWithDelimiter);

            //try
            //{
                EditPlannedProjectDto editPlannedProjectDto = new(_sharedResourceViewModel.EditPlannedProjectId, Name, StartDate, needlesToCreate, yarnsToCreate, Description, PatternPdfPath, _sharedResourceViewModel.UserId);
                ValidationResult validation = await _editPlannedProjectDtoValidator.ValidateAsync(editPlannedProjectDto);
                if (!validation.IsValid)
                {
                    string errorMessage = validation.Errors.GetMessagesAsString();
                    MessageBox.Show(errorMessage);
                    return;
                }
                //if user changes pattern pdf, then old one should be deleted
                if(!string.IsNullOrEmpty(_originalPatternPdfPath) && _originalPatternPdfPath != PatternPdfPath)
                {
                    _sharedResourceViewModel.FilesToDelete.Add(_originalPatternPdfPath);
                }
                await _projectService.EditPlannedProjectAsync(editPlannedProjectDto);
                _sharedResourceViewModel.OnPlannedProjectEdited();
                MessageBox.Show(Translations.PlannedProjectEditedSuccessfully);
            //}
            //catch (Exception exception)
            //{
            //    _logger.LogError(exception, "Error while editing planned project");
            //    MessageBox.Show(Translations.ErrorWhileEditingPlannedProject);
            //}
        }
    }
}