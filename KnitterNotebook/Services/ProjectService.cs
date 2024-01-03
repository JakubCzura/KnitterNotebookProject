using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Exceptions.Messages;
using KnitterNotebook.Expressions;
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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KnitterNotebook.Services;

public class ProjectService(DatabaseContext databaseContext,
                            IUserService userService) : CrudService<Project>(databaseContext), IProjectService
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IUserService _userService = userService;

    public async Task<bool> ProjectExistsAsync(int id) => await _databaseContext.Projects.AsNoTracking().AnyAsync(x => x.Id == id);

    /// <summary>
    /// Creates new project and saves it to database
    /// </summary>
    /// <param name="planProjectDto">Data to create</param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="NullReferenceException">When <paramref name="planProjectDto"/> is null</exception>
    /// <exception cref="EntityNotFoundException">When user doesn't exist in database</exception>
    public async Task<int> PlanProjectAsync(PlanProjectDto planProjectDto)
    {
        //User can plan project without .pdf file with pattern, but it would be impossible situation if nickname was null so exception is thrown when nickname is null
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

        if (!string.IsNullOrWhiteSpace(planProjectDto.SourcePatternPdfPath) && File.Exists(planProjectDto.SourcePatternPdfPath) && !string.IsNullOrWhiteSpace(destinationPatternPdfPath))
        {
            FileHelper.CopyWithDirectoryCreation(planProjectDto.SourcePatternPdfPath, destinationPatternPdfPath);
        }

        await _databaseContext.Projects.AddAsync(project);
        return await _databaseContext.SaveChangesAsync();
    }

    public async Task<PlannedProjectDto?> GetPlannedProjectAsync(int id)
    {
        IQueryable<Project> query = _databaseContext.Projects.AsNoTracking();

        foreach (Expression<Func<Project, object>> item in ProjectExpressions.IncludeNeedlesYarnsPattern)
        {
            query = query.Include(item);
        }

        return await query.FirstOrDefaultAsync(x => x.Id == id) is Project project ? new PlannedProjectDto(project) : null;
    }

    public async Task<List<PlannedProjectDto>> GetUserPlannedProjectsAsync(int userId)
    {
        IQueryable<Project> query = _databaseContext.Projects.AsNoTracking();

        foreach (Expression<Func<Project, object>> item in ProjectExpressions.IncludeNeedlesYarnsPattern)
        {
            query = query.Include(item);
        }

        return await query.Where(ProjectExpressions.GetUserProjectByStatus(userId, ProjectStatusName.Planned))
                          .Select(x => new PlannedProjectDto(x))
                          .ToListAsync();
    }

    public async Task<ProjectInProgressDto?> GetProjectInProgressAsync(int id)
    {
        IQueryable<Project> query = _databaseContext.Projects.AsNoTracking();

        foreach (Expression<Func<Project, object>> item in ProjectExpressions.IncludeNeedlesYarnsPatternImages)
        {
            query = query.Include(item);
        }  

        return await query.FirstOrDefaultAsync(x => x.Id == id) is Project project ? new ProjectInProgressDto(project) : null;
    }

    public async Task<List<ProjectInProgressDto>> GetUserProjectsInProgressAsync(int userId)
    {
        IQueryable<Project> query = _databaseContext.Projects.AsNoTracking();

        foreach (Expression<Func<Project, object>> item in ProjectExpressions.IncludeNeedlesYarnsPatternImages)
        {
            query = query.Include(item);
        }

        return await query.Where(ProjectExpressions.GetUserProjectByStatus(userId, ProjectStatusName.InProgress))
                          .Select(x => new ProjectInProgressDto(x))
                          .ToListAsync();
    }

    public async Task<FinishedProjectDto?> GetFinishedProjectAsync(int id)
    {
        IQueryable<Project> query = _databaseContext.Projects.AsNoTracking();

        foreach (Expression<Func<Project, object>> item in ProjectExpressions.IncludeNeedlesYarnsPatternImages)
        {
            query = query.Include(item);
        }

        return await query.FirstOrDefaultAsync(x => x.Id == id) is Project project ? new FinishedProjectDto(project) : null;
    }

    public async Task<List<FinishedProjectDto>> GetUserFinishedProjectsAsync(int userId)
    {
        IQueryable<Project> query = _databaseContext.Projects.AsNoTracking();

        foreach (Expression<Func<Project, object>> item in ProjectExpressions.IncludeNeedlesYarnsPatternImages)
        {
            query = query.Include(item);
        }

        return await query.Where(ProjectExpressions.GetUserProjectByStatus(userId, ProjectStatusName.Finished))
                          .Select(x => new FinishedProjectDto(x))
                          .ToListAsync();
    }

    /// <summary>
    /// Changes project's status and saves to database
    /// </summary>
    /// <param name="changeProjectStatusDto">Data to update</param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="InvalidOperationException">When <paramref name="changeProjectStatusDto"/> is null</exception>"
    public async Task<int> ChangeProjectStatus(ChangeProjectStatusDto changeProjectStatusDto)
    {
        Project? project = await _databaseContext.Projects.FirstOrDefaultAsync(x => x.Id == changeProjectStatusDto.ProjectId);

        if (project is null)
        {
            return 0;
        }

        project.AdjustProjectWhenChangingStatus(changeProjectStatusDto.ProjectStatus);

        _databaseContext.Update(project);
        return await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    /// Updates user's planned projects status to InProgress if projects' start date is today or earlier
    /// </summary>
    /// <param name="userId">User's id</param>
    /// <returns>The total number of rows updated in the database</returns>
    public async Task<int> ChangeUserPlannedProjectsToProjectsInProgressDueToDate(int userId)
        => await _databaseContext.Projects.Where(x => x.UserId == userId && x.ProjectStatus == ProjectStatusName.Planned && x.StartDate.HasValue && x.StartDate.Value.CompareTo(DateTime.Today) <= 0)
                                 .ExecuteUpdateAsync(x => x.SetProperty(y => y.ProjectStatus, ProjectStatusName.InProgress));

    /// <summary>
    /// Edits project and saves it to database
    /// </summary>
    /// <param name="editProjectDto">Data to edit</param>
    /// <returns>0 if project doesn't exists in database, otherwise quantity of entities saved to database</returns>
    /// <exception cref="EntityNotFoundException">If user doesn't exist in database</exception>
    public async Task<int> EditProjectAsync(EditProjectDto editProjectDto)
    {
        //User can edit project without .pdf file with pattern, but it would be impossible situation if nickname was null so exception is thrown when nickname is null
        string nickname = await _userService.GetNicknameAsync(editProjectDto.UserId)
                         ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(editProjectDto.UserId));

        Project? project = await _databaseContext.Projects
                                                 .Include(p => p.PatternPdf)
                                                 .Include(p => p.Needles)
                                                 .Include(p => p.Yarns)
                                                 .FirstOrDefaultAsync(p => p.Id == editProjectDto.Id);
        if (project == null)
        {
            return 0;
        }

        project.Name = editProjectDto.Name;
        project.StartDate = editProjectDto.StartDate;
        project.Description = editProjectDto.Description;

        if (editProjectDto.Needles.NotNullAndHaveAnyElement())
        {
            project.Needles.Clear();
            project.Needles.AddRange(editProjectDto.Needles.Select(x => new Needle(x.Size, x.SizeUnit)));
        }

        if (editProjectDto.Yarns.NotNullAndHaveAnyElement())
        {
            project.Yarns.Clear();
            project.Yarns.AddRange(editProjectDto.Yarns.Select(x => new Yarn(x.Name)));
        }

        //Prevent editing PatternPdf if user didn't choose new file or didn't delete old file
        if (editProjectDto.SourcePatternPdfPath != project.PatternPdf?.Path)
        {
            string? destinationPatternPdfPath = Paths.PathToSaveUserFile(nickname, Path.GetFileName(editProjectDto.SourcePatternPdfPath));
            PatternPdf? patternPdf = !string.IsNullOrWhiteSpace(destinationPatternPdfPath) ? new(destinationPatternPdfPath) : null;
            project.PatternPdf = patternPdf;

            if (!string.IsNullOrWhiteSpace(editProjectDto.SourcePatternPdfPath) && File.Exists(editProjectDto.SourcePatternPdfPath) && !string.IsNullOrWhiteSpace(destinationPatternPdfPath))
            {
                FileHelper.CopyWithDirectoryCreation(editProjectDto.SourcePatternPdfPath, destinationPatternPdfPath);
            }
        }

        _databaseContext.Update(project);
        return await _databaseContext.SaveChangesAsync();
    }
}