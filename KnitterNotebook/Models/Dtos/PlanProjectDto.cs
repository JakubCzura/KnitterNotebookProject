using System;
using System.Collections.Generic;

namespace KnitterNotebook.Models.Dtos
{
    public record PlanProjectDto
    (
        string Name,
        DateTime? StartDate,
        string YarnName,
        string PatternName,
        IEnumerable<CreateNeedleDto> Needles,
        IEnumerable<string> YarnNames,
        string Description,
        string PatternPdfPath,
        int UserId
    );
} 