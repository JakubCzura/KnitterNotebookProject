using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IProjectService : ICrudService<Project>
    {
        Task<bool> ProjectExistsAsync(int id);

        Task PlanProjectAsync(PlanProjectDto planProjectDto);

        Task<List<Project>> GetUserProjectsAsync(int userId);

        Task<List<PlannedProjectDto>> GetUserPlannedProjectsAsync(int userId);

        Task<List<ProjectInProgressDto>> GetUserProjectsInProgressAsync(int userId);

        Task<List<FinishedProjectDto>> GetUserFinishedProjectsAsync(int userId);

        Task ChangeProjectStatus(int userId, int projectId, ProjectStatusName projectStatusName);

        Task<ProjectInProgressDto?> GetProjectInProgressAsync(int id);
    }
}