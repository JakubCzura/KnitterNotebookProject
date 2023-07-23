using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IProjectService : ICrudService<Project>
    {
        Task PlanProjectAsync(PlanProjectDto planProjectDto);
    }
}