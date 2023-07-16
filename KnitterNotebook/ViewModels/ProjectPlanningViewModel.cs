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
        private string _yarnName = string.Empty;

        [ObservableProperty]
        private string _patternName = string.Empty;

        public List<Needle> Needles { get; set; } = new();

        public List<Yarn> Yarns { get; set; } = new();

        //Tzw. "inne"
        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        public string? _patternPdfPath = null;

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