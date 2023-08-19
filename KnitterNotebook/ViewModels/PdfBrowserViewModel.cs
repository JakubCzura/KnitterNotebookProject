using CommunityToolkit.Mvvm.ComponentModel;

namespace KnitterNotebook.ViewModels
{
    public partial class PdfBrowserViewModel : BaseViewModel
    {
        private readonly SharedResourceViewModel _sharedResourceViewModel;

        public PdfBrowserViewModel(SharedResourceViewModel sharedResourceViewModel)
        {
            _sharedResourceViewModel = sharedResourceViewModel;
            _sharedResourceViewModel.PatternPdfPathChanged += () => PdfPath = _sharedResourceViewModel.PatternPdfPath;
        }

        [ObservableProperty]
        private string? _pdfPath;
    }
}