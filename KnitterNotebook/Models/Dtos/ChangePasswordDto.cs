namespace KnitterNotebook.Models.Dtos;

public record ChangePasswordDto(int UserId, string NewPassword, string ConfirmPassword);