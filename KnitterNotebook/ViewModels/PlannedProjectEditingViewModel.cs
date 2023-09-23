using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services.Interfaces;
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
        public PlannedProjectEditingViewModel(ILogger<PlannedProjectEditingViewModel> logger, IProjectService projectService, SharedResourceViewModel sharedResourceViewModel)
        {
            _logger = logger;
            _projectService = projectService;
            _sharedResourceViewModel = sharedResourceViewModel;
        }

        private readonly ILogger<PlannedProjectEditingViewModel> _logger;
        private readonly IProjectService _projectService;
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
            PlannedProjectDto? plannedProjectDto = await _projectService.GetPlannedProjectAsync(_sharedResourceViewModel.EditPlannedProjectId);
            if (plannedProjectDto == null)
            {
                MessageBox.Show("Exception while getting project's data");
                Closewindow(PlannedProjectEditingWindow.Instance);
                return;
            }
            Name = plannedProjectDto.Name;
            StartDate = plannedProjectDto.StartDate;
            YarnsNamesWithDelimiter = string.Join(',', plannedProjectDto.Yarns.Select(x => x.Name));
            Description = plannedProjectDto.Description;
            PatternPdfPath = plannedProjectDto.PatternPdfPath;
            List<NeedleDto> needles = plannedProjectDto.Needles;
            if (needles.Count >= 1)
            {
                Needle1.Size = needles[0].Size;
                Needle1.SizeUnit = needles[0].SizeUnit;
                OnPropertyChanged(nameof(Needle1));
            }
            if (needles.Count >= 2)
            {
                Needle2.Size = needles[1].Size;
                Needle2.SizeUnit = needles[1].SizeUnit;
                OnPropertyChanged(nameof(Needle2));
            }
            if (needles.Count >= 3)
            {
                Needle3.Size = needles[2].Size;
                Needle3.SizeUnit = needles[2].SizeUnit;
                OnPropertyChanged(nameof(Needle3));
            }
            if (needles.Count >= 4)
            {
                Needle4.Size = needles[3].Size;
                Needle4.SizeUnit = needles[3].SizeUnit;
                OnPropertyChanged(nameof(Needle4));
            }
            if (needles.Count == 5)
            {
                Needle5.Size = needles[4].Size;
                Needle5.SizeUnit = needles[4].SizeUnit;
                OnPropertyChanged(nameof(Needle5));
            }
        }
    }
}