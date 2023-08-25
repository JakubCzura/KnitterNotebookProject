using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
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

namespace KnitterNotebook.Services
{
    public class ProjectImageService : CrudService<ProjectImage>, IProjectImageService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IUserService _userService;
        public ProjectImageService(DatabaseContext databaseContext, IUserService userService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _userService = userService;
        }

        public async Task CreateAsync(CreateProjectImageDto addProjectImageDto)
        {
            string? nickname = await _userService.GetNicknameAsync(addProjectImageDto.UserId);
            string? destinationImagePath = Paths.PathToSaveUserFile(nickname, Path.GetFileName(addProjectImageDto.ImagePath));

            if (string.IsNullOrWhiteSpace(destinationImagePath)) throw new ArgumentNullException(nameof(destinationImagePath), "Nie udało się zapisać zdjęcia.");

            ProjectImage projectImage = new()
            {
                Path = destinationImagePath,
                DateTime = DateTime.Now,
                ProjectId = addProjectImageDto.ProjectId
            };

            if (!string.IsNullOrWhiteSpace(addProjectImageDto.ImagePath) && !string.IsNullOrWhiteSpace(destinationImagePath))
                FileHelper.CopyWithDirectoryCreation(addProjectImageDto.ImagePath, destinationImagePath);

            await _databaseContext.AddAsync(projectImage);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<ProjectImageDto>> GetProjectImagesAsync(int projectId)
            => await _databaseContext.ProjectImages.Where(x => x.ProjectId == projectId)
                                                   .Select(x => new ProjectImageDto(x)).ToListAsync();
    }
}