namespace KnitterNotebook.ViewModels;

/// <summary>
/// View model for PdfBrowserWindow.xaml
/// </summary>
public partial class PdfBrowserViewModel : BaseViewModel
{
    public PdfBrowserViewModel(SharedResourceViewModel sharedResourceViewModel)
    {
        PdfPath = sharedResourceViewModel.PatternPdfPath;
    }

    public string? PdfPath { get; }
}