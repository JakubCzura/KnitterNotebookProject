using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces;

public interface IProjectImageService : ICrudService<ProjectImage>
{
    Task<int> CreateAsync(CreateProjectImageDto addProjectImageDto);

    Task<List<ProjectImageDto>> GetProjectImagesAsync(int projectId);
}