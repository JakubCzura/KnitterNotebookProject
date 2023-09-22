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

public class ProjectImageService : CrudService<ProjectImage>, IProjectImageService
{
    private readonly DatabaseContext _databaseContext;
    private readonly IUserService _userService;

    public ProjectImageService(DatabaseContext databaseContext, IUserService userService) : base(databaseContext)
    {
        _databaseContext = databaseContext;
        _userService = userService;
    }

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

        if (string.IsNullOrWhiteSpace(destinationImagePath)) throw new ArgumentNullException(nameof(destinationImagePath), ExceptionsMessages.NullFileWhenSave);

        ProjectImage projectImage = new()
        {
            Path = destinationImagePath,
            ProjectId = addProjectImageDto.ProjectId
        };

        if (!string.IsNullOrWhiteSpace(addProjectImageDto.ImagePath) && !string.IsNullOrWhiteSpace(destinationImagePath))
            FileHelper.CopyWithDirectoryCreation(addProjectImageDto.ImagePath, destinationImagePath);

        await _databaseContext.AddAsync(projectImage);
        return await _databaseContext.SaveChangesAsync();
    }

    public async Task<List<ProjectImageDto>> GetProjectImagesAsync(int projectId)
        => await _databaseContext.ProjectImages.AsNoTracking()
                                               .Where(x => x.ProjectId == projectId)
                                               .Select(x => new ProjectImageDto(x)).ToListAsync();
}