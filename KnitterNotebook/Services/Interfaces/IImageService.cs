using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IImageService : ICrudService<Image>
    {
        Task CreateAsync(CreateImageDto createImageDto);
    }
}