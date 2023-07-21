﻿using KnitterNotebook.Database;
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
        private readonly IUserService _userService;
        private readonly IProjectStatusService _projectStatusService;
        public ProjectService(DatabaseContext databaseContext, IUserService userService, IProjectStatusService projectStatusService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _userService = userService;
            _projectStatusService = projectStatusService;
        }

        public async Task PlanProjectAsync(PlanProjectDto planProjectDto)
        {
            List<Needle> needles = planProjectDto.Needles.Select(x => new Needle() { Size = x.Size, SizeUnit = x.Unit }).ToList();
            List<Yarn> yarns = planProjectDto.YarnsNames.Select(x => new Yarn() { Name = x }).ToList();

            int projectStatusId = planProjectDto.StartDate.HasValue && planProjectDto.StartDate.Value.CompareTo(DateTime.Today) >= 0 ? 2 : 1; 
            ProjectStatus projectStatus = await _projectStatusService.GetAsync(projectStatusId)! ?? throw new NullReferenceException("Project status can not be null");

            User user = await _userService.GetAsync(planProjectDto.UserId)! ?? throw new NullReferenceException("User can not be null"); ;
            Project project = new()
            {
                Name = planProjectDto.Name,
                StartDate = planProjectDto.StartDate,
                PatternName = planProjectDto.PatternName,
                Needles = needles,
                Yarns = yarns,
                Description = planProjectDto.Description,
                ProjectStatus = projectStatus,
                PatternPdfPath = planProjectDto.PatternPdfPath,
                User = user
            };
            
            await _databaseContext.Projects.AddAsync(project);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
