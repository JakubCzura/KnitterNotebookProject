using System;
using System.Collections.Generic;

namespace KnitterNotebook.Models.Dtos;

public record PlanProjectDto(string Name,
                             DateTime? StartDate,
                             IEnumerable<CreateNeedleDto> Needles,
                             IEnumerable<CreateYarnDto> Yarns,
                             string? Description,
                             string? SourcePatternPdfPath,
                             int UserId );