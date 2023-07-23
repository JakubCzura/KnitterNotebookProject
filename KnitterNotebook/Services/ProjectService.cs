using KnitterNotebook.Database;
using KnitterNotebook.Helpers;
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

        public ProjectService(DatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task PlanProjectAsync(PlanProjectDto planProjectDto)
        {
            List<Needle> needles = planProjectDto.Needles.Select(x => new Needle() { Size = x.Size, SizeUnit = x.SizeUnit }).ToList();
            List<Yarn> yarns = planProjectDto.Yarns.Select(x => new Yarn() { Name = x.Name }).ToList();

            int projectStatusId = planProjectDto.StartDate.HasValue && planProjectDto.StartDate.Value.CompareTo(DateTime.Today) >= 0 ? 2 : 1; 

            Project project = new()
            {
                Name = planProjectDto.Name,
                StartDate = planProjectDto.StartDate,
                PatternName = planProjectDto.PatternName,
                Needles = needles,
                Yarns = yarns,
                Description = planProjectDto.Description,
                ProjectStatusId = projectStatusId,
                PatternPdfPath = planProjectDto.PatternPdfPath,
                UserId = planProjectDto.UserId
            };
            
            await _databaseContext.Projects.AddAsync(project);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
