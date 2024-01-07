using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Exceptions.Messages;
using KnitterNotebook.Helpers;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services;

public class ProjectImageService(DatabaseContext databaseContext,
                                 IUserService userService) : CrudService<ProjectImage>(databaseContext), IProjectImageService
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IUserService _userService = userService;

    /// <summary>
    /// Adds project's new image to database and copies image to user's folder
    /// </summary>
    /// <param name="addProjectImageDto"></param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="ArgumentNullException">If destinationImagePath is null</exception>
    /// <exception cref="NullReferenceException">If dto is null</exception>
    public async Task<int> CreateAsync(CreateProjectImageDto addProjectImageDto)
    {
        string? nickname = await _userService.GetNicknameAsync(addProjectImageDto.UserId);
        string? destinationImagePath = Paths.PathToSaveUserFile(nickname, Path.GetFileName(addProjectImageDto.ImagePath));

        if (string.IsNullOrWhiteSpace(destinationImagePath))
        {
            throw new ArgumentNullException(destinationImagePath, ExceptionsMessages.NullFileWhenSave);
        }

        ProjectImage projectImage = new()
        {
            Path = destinationImagePath,
            ProjectId = addProjectImageDto.ProjectId
        };

        if (!string.IsNullOrWhiteSpace(addProjectImageDto.ImagePath) && File.Exists(addProjectImageDto.ImagePath) && !string.IsNullOrWhiteSpace(destinationImagePath))
        {
            FileHelper.CopyWithDirectoryCreation(addProjectImageDto.ImagePath, destinationImagePath);
        }

        await _databaseContext.AddAsync(projectImage);
        return await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    /// Returns all images for project with given <paramref name="projectId"/> from database
    /// </summary>
    /// <param name="projectId">Project's id</param>
    /// <returns>All images of specified project</returns>
    public async Task<List<ProjectImageDto>> GetProjectImagesAsync(int projectId)
        => await _databaseContext.ProjectImages.AsNoTracking()
                                               .Where(x => x.ProjectId == projectId)
                                               .Select(x => new ProjectImageDto(x))
                                               .ToListAsync();
}