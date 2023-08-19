using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Helpers;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Models.Enums;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class ProjectService : CrudService<Project>, IProjectService
    {
        private readonly DatabaseContext _databaseContext;

        public ProjectService(DatabaseContext databaseContext, IUserService userService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _userService = userService;
        }

        private readonly IUserService _userService;

        public async Task PlanProjectAsync(PlanProjectDto planProjectDto)
        {
            string? nickname = await _userService.GetNicknameAsync(LoggedUserInformation.Id);
            string? destinationPatternPdfPath = Paths.PathToSaveUserFile(nickname, Path.GetFileName(planProjectDto.SourcePatternPdfPath));

            List<Needle> needles = planProjectDto.Needles.Select(x => new Needle(x.Size, x.SizeUnit)).ToList();
            List<Yarn> yarns = planProjectDto.Yarns.Select(x => new Yarn(x.Name)).ToList();

            PatternPdf? patternPdf = !string.IsNullOrWhiteSpace(destinationPatternPdfPath) ? new(destinationPatternPdfPath) : null;

            //If planned project's start date is today then projectStatus is InProgress, otherwise it is Planned
            ProjectStatusName projectStatus = planProjectDto.StartDate.HasValue && planProjectDto.StartDate.Value.CompareTo(DateTime.Today) == 0
                                                ? ProjectStatusName.InProgress
                                                : ProjectStatusName.Planned;

            Project project = new()
            {
                Name = planProjectDto.Name,
                StartDate = planProjectDto.StartDate,
                Needles = needles,
                Yarns = yarns,
                Description = planProjectDto.Description,
                ProjectStatus = projectStatus,
                PatternPdf = patternPdf,
                UserId = planProjectDto.UserId
            };

            if (!string.IsNullOrWhiteSpace(planProjectDto.SourcePatternPdfPath) && !string.IsNullOrWhiteSpace(destinationPatternPdfPath))
                FileHelper.CopyWithDirectoryCreation(planProjectDto.SourcePatternPdfPath, destinationPatternPdfPath);

            await _databaseContext.Projects.AddAsync(project);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<Project>> GetUserProjectsAsync(int userId)
            => await _databaseContext.Projects.Include(x => x.Needles)
                                              .Include(x => x.Yarns)
                                              .Include(x => x.PatternPdf)
                                              .Where(x => x.UserId == userId).ToListAsync();

        public async Task<List<PlannedProjectDto>> GetUserPlannedProjectsAsync(int userId)
           => await _databaseContext.Projects.Include(x => x.Needles)
                                             .Include(x => x.Yarns)
                                             .Include(x => x.PatternPdf)
                                             .Where(x => x.UserId == userId && x.ProjectStatus == ProjectStatusName.Planned)
                                             .Select(x => new PlannedProjectDto(x)).ToListAsync();

        public async Task<List<ProjectInProgressDto>> GetUserProjectsInProgressAsync(int userId)
          => await _databaseContext.Projects.Include(x => x.Needles)
                                            .Include(x => x.Yarns)
                                            .Include(x => x.PatternPdf)
                                            .Include(x => x.ProjectImages)
                                            .Where(x => x.UserId == userId && x.ProjectStatus == ProjectStatusName.InProgress)
                                            .Select(x => new ProjectInProgressDto(x)).ToListAsync();

        public async Task<List<FinishedProjectDto>> GetUserFinishedProjectsAsync(int userId)
          => await _databaseContext.Projects.Include(x => x.PatternPdf)
                                            .Include(x => x.ProjectImages)
                                            .Where(x => x.UserId == userId && x.ProjectStatus == ProjectStatusName.Finished)
                                            .Select(x => new FinishedProjectDto(x)).ToListAsync();

        public async Task ChangeProjectStatus(int userId, int projectId, ProjectStatusName projectStatusName)
        {
            Project project = await _databaseContext.Projects.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == projectId)
                                    ?? throw new EntityNotFoundException("Id projektu jest nieodpowiednie lub nie odnaleziono użytkownika");

            project.ProjectStatus = projectStatusName;
            _databaseContext.Update(project);
            await _databaseContext.SaveChangesAsync();
        }
    }
}