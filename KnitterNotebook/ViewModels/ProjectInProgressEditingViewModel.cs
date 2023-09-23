using KnitterNotebook.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.ViewModels
{
    public partial class ProjectInProgressEditingViewModel : BaseViewModel
    {
        public ProjectInProgressEditingViewModel(ILogger<ProjectInProgressEditingViewModel> logger, IProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        private readonly ILogger<ProjectInProgressEditingViewModel> _logger;
        private readonly IProjectService _projectService;
    }
}
