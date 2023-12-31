namespace KnitterNotebook.Models.Dtos;

public record CreateMovieUrlDto(string Title,
                                string Link,
                                string? Description,
                                int UserId);