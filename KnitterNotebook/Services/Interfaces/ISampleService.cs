using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface ISampleService : ICrudService<Sample>
    {
        Task<bool> CreateAsync(CreateSampleDto createSampleDto);
    }
}