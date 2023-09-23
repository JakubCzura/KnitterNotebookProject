using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Exceptions.Messages;
using KnitterNotebook.Helpers;
using KnitterNotebook.Helpers.Extensions;
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

namespace KnitterNotebook.Services;

public class ProjectService : CrudService<Project>, IProjectService
{
    private readonly DatabaseContext _databaseContext;
    private readonly IUserService _userService;

    public ProjectService(DatabaseContext databaseContext, IUserService userService) : base(databaseContext)
    {
        _databaseContext = databaseContext;
        _userService = userService;
    }

    public async Task<bool> ProjectExistsAsync(int id) => await _databaseContext.Projects.AsNoTracking().AnyAsync(x => x.Id == id);

    /// <summary>
    /// Creates new project and saves to database
    /// </summary>
    /// <param name="planProjectDto">Data to create</param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="NullReferenceException">When <paramref name="planProjectDto"/> is null</exception>
    public async Task<int> PlanProjectAsync(PlanProjectDto planProjectDto)
    {
        string nickname = await _userService.GetNicknameAsync(planProjectDto.UserId)
                         ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(planProjectDto.UserId));

        string? destinationPatternPdfPath = Paths.PathToSaveUserFile(nickname, Path.GetFileName(planProjectDto.SourcePatternPdfPath));
        PatternPdf? patternPdf = !string.IsNullOrWhiteSpace(destinationPatternPdfPath) ? new(destinationPatternPdfPath) : null;

        List<Needle> needles = planProjectDto.Needles.Select(x => new Needle(x.Size, x.SizeUnit)).ToList();
        List<Yarn> yarns = planProjectDto.Yarns.Select(x => new Yarn(x.Name)).ToList();

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
        return await _databaseContext.SaveChangesAsync();
    }

    public async Task<PlannedProjectDto?> GetPlannedProjectAsync(int id)
    {
        Project? project = await _databaseContext.Projects.AsNoTracking()
                                                          .Include(x => x.Needles)
                                                          .Include(x => x.Yarns)
                                                          .Include(x => x.PatternPdf)
                                                          .FirstOrDefaultAsync(x => x.Id == id);

        return project is not null ? new PlannedProjectDto(project) : null;
    }

    public async Task<List<PlannedProjectDto>> GetUserPlannedProjectsAsync(int userId)
       => await _databaseContext.Projects.AsNoTracking()
                                         .Include(x => x.Needles)
                                         .Include(x => x.Yarns)
                                         .Include(x => x.PatternPdf)
                                         .Where(x => x.UserId == userId && x.ProjectStatus == ProjectStatusName.Planned)
                                         .Select(x => new PlannedProjectDto(x))
                                         .ToListAsync();

    public async Task<ProjectInProgressDto?> GetProjectInProgressAsync(int id)
    {
        Project? project = await _databaseContext.Projects.AsNoTracking()
                                                          .Include(x => x.Needles)
                                                          .Include(x => x.Yarns)
                                                          .Include(x => x.PatternPdf)
                                                          .Include(x => x.ProjectImages)
                                                          .FirstOrDefaultAsync(x => x.Id == id);

        return project is not null ? new ProjectInProgressDto(project) : null;
    }

    public async Task<List<ProjectInProgressDto>> GetUserProjectsInProgressAsync(int userId)
      => await _databaseContext.Projects.AsNoTracking()
                                        .Include(x => x.Needles)
                                        .Include(x => x.Yarns)
                                        .Include(x => x.PatternPdf)
                                        .Include(x => x.ProjectImages)
                                        .Where(x => x.UserId == userId && x.ProjectStatus == ProjectStatusName.InProgress)
                                        .Select(x => new ProjectInProgressDto(x))
                                        .ToListAsync();

    public async Task<FinishedProjectDto?> GetFinishedProjectAsync(int id)
    {
        Project? project = await _databaseContext.Projects.AsNoTracking()
                                                          .Include(x => x.Needles)
                                                          .Include(x => x.Yarns)
                                                          .Include(x => x.PatternPdf)
                                                          .Include(x => x.ProjectImages)
                                                          .FirstOrDefaultAsync(x => x.Id == id);

        return project is not null ? new FinishedProjectDto(project) : null;
    }

    public async Task<List<FinishedProjectDto>> GetUserFinishedProjectsAsync(int userId)
      => await _databaseContext.Projects.Include(x => x.Needles)
                                        .Include(x => x.Yarns)
                                        .Include(x => x.PatternPdf)
                                        .Include(x => x.ProjectImages)
                                        .Where(x => x.UserId == userId && x.ProjectStatus == ProjectStatusName.Finished)
                                        .Select(x => new FinishedProjectDto(x))
                                        .ToListAsync();

    /// <summary>
    /// Changes project's status and saves to database
    /// </summary>
    /// <param name="changeProjectStatusDto">Data to update</param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="InvalidOperationException">When <paramref name="changeProjectStatusDto"/> is null</exception>"
    public async Task<int> ChangeProjectStatus(ChangeProjectStatusDto changeProjectStatusDto)
    {
        Project? project = await _databaseContext.Projects.FirstOrDefaultAsync(x => x.Id == changeProjectStatusDto.ProjectId);

        if (project is null) return 0;

        project.AdjustProjectWhenChangingStatus(changeProjectStatusDto.ProjectStatus);

        _databaseContext.Update(project);
        return await _databaseContext.SaveChangesAsync();
    }

    public async Task<int> EditPlannedProjectAsync(EditPlannedProjectDto editPlannedProjectDto)
    {
        throw new NotImplementedException("EditPlannedProjectAsync");
        //string nickname = await _userService.GetNicknameAsync(editPlannedProjectDto.UserId)
        //                 ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(editPlannedProjectDto.UserId));

        //string? destinationPatternPdfPath = Paths.PathToSaveUserFile(nickname, Path.GetFileName(editPlannedProjectDto.SourcePatternPdfPath));
        //PatternPdf? patternPdf = !string.IsNullOrWhiteSpace(destinationPatternPdfPath) ? new(destinationPatternPdfPath) : null;

        //List<Needle> needles = editPlannedProjectDto.Needles.Select(x => new Needle(x.Size, x.SizeUnit)).ToList();
        //List<Yarn> yarns = editPlannedProjectDto.Yarns.Select(x => new Yarn(x.Name)).ToList();

        //if (!string.IsNullOrWhiteSpace(editPlannedProjectDto.SourcePatternPdfPath) && !string.IsNullOrWhiteSpace(destinationPatternPdfPath))
        //    FileHelper.CopyWithDirectoryCreation(editPlannedProjectDto.SourcePatternPdfPath, destinationPatternPdfPath);

        //Project? project = await _databaseContext.Projects.FindAsync(editPlannedProjectDto.Id);
        //if (project == null)
        //{
        //    return 0;
        //}
        //project.Name = editPlannedProjectDto.Name;
        //project.StartDate = editPlannedProjectDto.StartDate;
        //project.Needles = needles;
        //project.Yarns = yarns;
        //project.Description = editPlannedProjectDto.Description;
        //project.PatternPdf = patternPdf;

        //_databaseContext.Update(project);
        //return await _databaseContext.SaveChangesAsync();
    }
}