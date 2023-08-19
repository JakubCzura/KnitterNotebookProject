using CommunityToolkit.Mvvm.ComponentModel;

namespace KnitterNotebook.ViewModels
{
    public partial class PdfBrowserViewModel : BaseViewModel
    {
        public PdfBrowserViewModel(SharedResourceViewModel sharedResourceViewModel)
        {
            PdfPath = sharedResourceViewModel.PatternPdfPath;
        }

        [ObservableProperty]
        private string? _pdfPath;
    }
}