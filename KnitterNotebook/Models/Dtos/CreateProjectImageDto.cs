namespace KnitterNotebook.Models.Dtos;

public record CreateProjectImageDto(int ProjectId,
                                    string ImagePath,
                                    int UserId);