using CommunityToolkit.Mvvm.Input;
using KnitterNotebook.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace KnitterNotebook.ViewModels
{
    public partial class PlannedProjectEditingViewModel : BaseViewModel
    {
        public PlannedProjectEditingViewModel(ILogger<PlannedProjectEditingViewModel> logger, IProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        private readonly ILogger<PlannedProjectEditingViewModel> _logger;
        private readonly IProjectService _projectService;

        [RelayCommand]
        private async Task OnLoadedWindowAsync()
        {

        }
    }
}