using System;
using System.Collections.Generic;

namespace KnitterNotebook.Models.Dtos;

public record EditProjectDto(int Id,
                             string Name,
                             DateTime? StartDate,
                             IEnumerable<CreateNeedleDto> Needles,
                             IEnumerable<CreateYarnDto> Yarns,
                             string? Description,
                             string? SourcePatternPdfPath,
                             int UserId) : PlanProjectDto(Name, StartDate, Needles, Yarns, Description, SourcePatternPdfPath, UserId);