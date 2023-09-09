using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IProjectService : ICrudService<Project>
    {
        Task<bool> ProjectExistsAsync(int id);

        Task PlanProjectAsync(PlanProjectDto planProjectDto);

        Task<List<Project>> GetUserProjectsAsync(int userId);

        Task<PlannedProjectDto?> GetPlannedProjectAsync(int id);

        Task<List<PlannedProjectDto>> GetUserPlannedProjectsAsync(int userId);

        Task<ProjectInProgressDto?> GetProjectInProgressAsync(int id);

        Task<List<ProjectInProgressDto>> GetUserProjectsInProgressAsync(int userId);

        Task<FinishedProjectDto?> GetFinishedProjectAsync(int id);

        Task<List<FinishedProjectDto>> GetUserFinishedProjectsAsync(int userId);

        Task ChangeProjectStatus(ChangeProjectStatusDto changeProjectStatusDto);
    }
}