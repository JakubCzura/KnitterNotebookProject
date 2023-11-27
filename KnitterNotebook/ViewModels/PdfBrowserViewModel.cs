namespace KnitterNotebook.ViewModels;

/// <summary>
/// View model for PdfBrowserWindow.xaml
/// </summary>
public partial class PdfBrowserViewModel(SharedResourceViewModel sharedResourceViewModel) : BaseViewModel
{
    public string? PdfPath { get; } = sharedResourceViewModel.PatternPdfPath;
}