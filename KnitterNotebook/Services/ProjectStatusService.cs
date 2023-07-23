using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Services
{
    public class ProjectStatusService : CrudService<ProjectStatus>, IProjectStatusService
    {
        public ProjectStatusService(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}