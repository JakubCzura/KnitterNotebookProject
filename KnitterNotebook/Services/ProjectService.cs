using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class ProjectService : CrudService<Project>, IProjectService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IUserService _userService;

        public ProjectService(DatabaseContext databaseContext, IUserService userService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _userService = userService;
        }

        public Task PlanProjectAsync(PlanProjectDto planProjectDto)
        {
            throw new NotImplementedException();
        }
    }
}
