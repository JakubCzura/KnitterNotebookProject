using System;
using System.Collections.Generic;

namespace KnitterNotebook.Models.Dtos
{
    public record PlanProjectDto
    (
        string Name,
        DateTime? StartDate,
        string PatternName,
        IEnumerable<CreateNeedleDto> Needles,
        IEnumerable<CreateYarnDto> Yarns,
        string? Description,
        string? PatternPdfPath,
        int UserId
    );
}