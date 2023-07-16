using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace KnitterNotebook.ViewModels
{
    public partial class ProjectPlanningViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private DateTime? _startDate = null;

        [ObservableProperty]
        private string _yarnName = string.Empty;

        [ObservableProperty]
        private string _patternName = string.Empty;

        ////public virtual List<Needle> Needles { get; set; } = new();

        ////public virtual List<Yarn> Yarns { get; set; } = new();

        //Tzw. "inne"
        [ObservableProperty]
        private string _description = string.Empty;

        //public ProjectStatus ProjectStatus { get; set; } = new();

        ////public string PDFFilePath { get; set; } = string.Empty;

        ////public virtual List<ProjectImage> ProjectImages = new();

        [ObservableProperty]
        private string _userId = string.Empty;
    }
}