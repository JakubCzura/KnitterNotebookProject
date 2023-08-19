using CommunityToolkit.Mvvm.ComponentModel;

namespace KnitterNotebook.ViewModels
{
    public partial class SharedResourceViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string? _patternPdfPath;
    }
}