using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Dtos;

public record ChangeProjectStatusDto(int ProjectId, ProjectStatusName ProjectStatus);