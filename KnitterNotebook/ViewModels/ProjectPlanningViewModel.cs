using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Helpers;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
    public partial class ProjectPlanningViewModel : ObservableObject
    {
        public ProjectPlanningViewModel()
        {
            DeletePatternPdfCommand = new RelayCommand(() => PatternPdfPath = null);
        }

        public ICommand DeletePatternPdfCommand { get; }

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private DateTime? _startDate = null;

        [ObservableProperty]
        public List<string> _yarnSemicolonList = new();

        [ObservableProperty]
        private string _patternName = string.Empty;

        public List<Needle> Needles { get; set; } = new();

        public List<Yarn> Yarns { get; set; } = new();

        //Tzw. "inne"
        [ObservableProperty]
        private string? _description = null;

        [ObservableProperty]
        public string? _patternPdfPath = null;

        [ObservableProperty]
        public NeedleToPlanProjectDto _needle1 = new();

        [ObservableProperty]
        public NeedleToPlanProjectDto _needle2 = new();

        [ObservableProperty]
        public NeedleToPlanProjectDto _needle3 = new();

        [ObservableProperty]
        public NeedleToPlanProjectDto _needle4 = new();

        [ObservableProperty]
        public NeedleToPlanProjectDto _needle5 = new();

        public static IEnumerable<string> NeedleSizeUnitList => NeedleSizeUnits.UnitsList;

        [RelayCommand]
        private void ChoosePatternPdf()
        {
            OpenFileDialog dialog = new()
            {
                Filter = "Image Files (*.pdf)|*.pdf"
            };
            dialog.ShowDialog();
            PatternPdfPath = dialog.FileName;
        }

        [RelayCommand]
        private async Task AddProjectAsync()
        {
            IEnumerable<NeedleToPlanProjectDto> needles = NeedlesToPlanProjectFilter.GetNeedlesWithSizeHasValue(Needle1, Needle2, Needle3, Needle4, Needle5);
        }
    }
}