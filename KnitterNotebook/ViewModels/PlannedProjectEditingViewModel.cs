using KnitterNotebook.Services.Interfaces;
using Microsoft.Extensions.Logging;

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


    }
}