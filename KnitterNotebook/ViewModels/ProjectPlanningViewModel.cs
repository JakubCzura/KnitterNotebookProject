using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Models;
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
        public double? _needleSize1 = null;

        [ObservableProperty]
        public string _needleSizeUnit1 = "mm";

        [ObservableProperty]
        public double? _needleSize2 = null;

        [ObservableProperty]
        public string _needleSizeUnit2 = "mm";

        [ObservableProperty]
        public double? _needleSize3 = null;

        [ObservableProperty]
        public string _needleSizeUnit3 = "mm";

        [ObservableProperty]
        public double? _needleSize4 = null;

        [ObservableProperty]
        public string _needleSizeUnit4 = "mm";

        [ObservableProperty]
        public double? _needleSize5 = null;

        [ObservableProperty]
        public string _needleSizeUnit5 = "mm";

        public List<string> NeedleSizeUnitList = new() { "mm", "cm" };

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

        }

        
    }
}