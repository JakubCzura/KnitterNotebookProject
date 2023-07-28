using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Helpers;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class ProjectService : CrudService<Project>, IProjectService
    {
        private readonly DatabaseContext _databaseContext;

        public ProjectService(DatabaseContext databaseContext, IUserService userService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _userService = userService;
        }

        private readonly IUserService _userService;

        public async Task PlanProjectAsync(PlanProjectDto planProjectDto)
        {
            string? nickname = await _userService.GetNicknameAsync(LoggedUserInformation.Id);
            string? destinationPatternPdfPath = Paths.PathToSaveUserFile(nickname, Path.GetFileName(planProjectDto.SourcePatternPdfPath));

            List<Needle> needles = planProjectDto.Needles.Select(x => new Needle(x.Size, x.SizeUnit)).ToList();
            List<Yarn> yarns = planProjectDto.Yarns.Select(x => new Yarn(x.Name)).ToList();

            PatternPdf? patternPdf = !string.IsNullOrWhiteSpace(destinationPatternPdfPath) ? new(destinationPatternPdfPath) : null;

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
                PatternPdf = patternPdf,
                UserId = planProjectDto.UserId
            };

            if (!string.IsNullOrWhiteSpace(planProjectDto.SourcePatternPdfPath) && !string.IsNullOrWhiteSpace(destinationPatternPdfPath))
                FileHelper.CopyWithDirectoryCreation(planProjectDto.SourcePatternPdfPath, destinationPatternPdfPath);

            await _databaseContext.Projects.AddAsync(project);
            await _databaseContext.SaveChangesAsync();
        }
    }
}