using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces;

public interface IProjectService : ICrudService<Project>
{
    Task<bool> ProjectExistsAsync(int id);

    Task<int> PlanProjectAsync(PlanProjectDto planProjectDto);

    Task<PlannedProjectDto?> GetPlannedProjectAsync(int id);

    Task<List<PlannedProjectDto>> GetUserPlannedProjectsAsync(int userId);

    Task<ProjectInProgressDto?> GetProjectInProgressAsync(int id);

    Task<List<ProjectInProgressDto>> GetUserProjectsInProgressAsync(int userId);

    Task<FinishedProjectDto?> GetFinishedProjectAsync(int id);

    Task<List<FinishedProjectDto>> GetUserFinishedProjectsAsync(int userId);

    Task<int> ChangeProjectStatus(ChangeProjectStatusDto changeProjectStatusDto);

    Task<int> EditProjectAsync(EditProjectDto editPlannedProjectDto);
}