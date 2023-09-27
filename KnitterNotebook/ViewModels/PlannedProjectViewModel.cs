using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Helpers.Filters;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Enums;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace KnitterNotebook.ViewModels
{
    public partial class PlannedProjectBaseViewModel : BaseViewModel
    {
        [ObservableProperty]
        protected string _name = string.Empty;

        [ObservableProperty]
        protected DateTime? _startDate = null;

        [ObservableProperty]
        protected string _yarnsNamesWithDelimiter = string.Empty;

        //Tzw. "inne"
        [ObservableProperty]
        protected string? _description = null;

        [ObservableProperty]
        protected string? _patternPdfPath = null;

        [ObservableProperty]
        protected NullableSizeNeedle _needle1 = new();

        [ObservableProperty]
        protected NullableSizeNeedle _needle2 = new();

        [ObservableProperty]
        protected NullableSizeNeedle _needle3 = new();

        [ObservableProperty]
        protected NullableSizeNeedle _needle4 = new();

        [ObservableProperty]
        protected NullableSizeNeedle _needle5 = new();

        public static IEnumerable<string> NeedleSizeUnitList => Enum.GetNames<NeedleSizeUnit>();

        [RelayCommand]
        protected void ChoosePatternPdf()
        {
            OpenFileDialog dialog = new()
            {
                Filter = FileDialogFilter.PdfFilter
            };
            dialog.ShowDialog();
            PatternPdfPath = dialog.FileName;
        }

        [RelayCommand]
        protected void DeletePatternPdf() => PatternPdfPath = null;
    }
}