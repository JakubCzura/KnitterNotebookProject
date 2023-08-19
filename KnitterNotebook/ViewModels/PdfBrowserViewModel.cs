using CommunityToolkit.Mvvm.ComponentModel;

namespace KnitterNotebook.ViewModels
{
    /// <summary>
    /// Class to share data and communicate between viewmodels
    /// </summary>
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