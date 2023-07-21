using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace KnitterNotebook.Services
{
    public class ProjectStatusService : CrudService<ProjectStatus>, IProjectStatusService
    {
        public ProjectStatusService(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
