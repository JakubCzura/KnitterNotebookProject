using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface ISampleService : ICrudService<Sample>
    {
        Task<int> CreateAsync(CreateSampleDto createSampleDto);

        Task<List<SampleDto>> GetUserSamplesAsync(int userId);
    }
}