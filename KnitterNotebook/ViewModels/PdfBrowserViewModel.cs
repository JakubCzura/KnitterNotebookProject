namespace KnitterNotebook.ViewModels;

public partial class PdfBrowserViewModel : BaseViewModel
{
    public PdfBrowserViewModel(SharedResourceViewModel sharedResourceViewModel)
    {
        PdfPath = sharedResourceViewModel.PatternPdfPath;
    }

    public string? PdfPath { get; }
}