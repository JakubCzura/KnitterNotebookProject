using KnitterNotebook.Database;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class ProjectImageService : CrudService<ProjectImage>, IProjectImageService
    {
        private readonly DatabaseContext _databaseContext;

        public ProjectImageService(DatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task CreateAsync(CreateProjectImageDto addProjectImageDto)
        {
            ProjectImage projectImage = new()
            {
                Path = addProjectImageDto.ImagePath,
                DateTime = DateTime.Now,
                ProjectId = addProjectImageDto.ProjectId
            };

            await _databaseContext.AddAsync(projectImage);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<ProjectImageDto>> GetProjectImagesAsync(int projectId)
            => await _databaseContext.ProjectImages.Where(x => x.ProjectId == projectId)
                                                   .Select(x => new ProjectImageDto(x)).ToListAsync();
    }
}